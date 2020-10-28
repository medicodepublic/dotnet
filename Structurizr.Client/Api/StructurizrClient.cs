﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Structurizr.Encryption;
using Structurizr.IO.Json;

namespace Structurizr.Api
{
    /// <summary>
    ///     A client for the Structurizr API (https://api.structurizr.com)
    ///     that allows you to get and put Structurizr workspaces in a JSON format.
    /// </summary>
    public class StructurizrClient
    {
        private const string WorkspacePath = "/workspace/";

        private string _apiKey;

        private string _apiSecret;

        private string _url;

        private readonly string _version;

        /// <summary>
        ///     Creates a new Structurizr API client with the specified API key and secret,
        ///     for the default API URL(https://api.structurizr.com).
        /// </summary>
        /// <param name="apiKey">The API key of your workspace.</param>
        /// <param name="apiSecret">The API secret of your workspace.</param>
        public StructurizrClient(string apiKey, string apiSecret) : this("https://api.structurizr.com", apiKey,
            apiSecret)
        {
        }

        /// <summary>
        ///     Creates a new Structurizr client with the specified API URL, key and secret.
        /// </summary>
        /// <param name="apiUrl">The URL of your Structurizr instance.</param>
        /// <param name="apiKey">The API key of your workspace.</param>
        /// <param name="apiSecret">The API secret of your workspace.</param>
        public StructurizrClient(string apiUrl, string apiKey, string apiSecret)
        {
            Url = apiUrl;
            ApiKey = apiKey;
            ApiSecret = apiSecret;

            WorkspaceArchiveLocation = new DirectoryInfo(".");
            MergeFromRemote = true;

            _version = typeof(StructurizrClient).GetTypeInfo().Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        }

        public string Url
        {
            get => _url;
            set
            {
                if (value == null || value.Trim().Length == 0)
                    throw new ArgumentException("The API URL must not be null or empty.");

                if (value.EndsWith("/"))
                    _url = value.Substring(0, value.Length - 1);
                else
                    _url = value;
            }
        }

        public string ApiKey
        {
            get => _apiKey;
            set
            {
                if (value == null || value.Trim().Length == 0)
                    throw new ArgumentException("The API key must not be null or empty.");

                _apiKey = value;
            }
        }

        public string ApiSecret
        {
            get => _apiSecret;
            set
            {
                if (value == null || value.Trim().Length == 0)
                    throw new ArgumentException("The API secret must not be null or empty.");

                _apiSecret = value;
            }
        }

        /// <summary>the location where a copy of the workspace will be archived when it is retrieved from the server</summary>
        public DirectoryInfo WorkspaceArchiveLocation { get; set; }

        public EncryptionStrategy EncryptionStrategy { get; set; }

        public bool MergeFromRemote { get; set; }

        /// <summary>
        ///     Locks the specified workspace.
        /// </summary>
        /// <param name="workspaceId">The workspace ID.</param>
        /// <returns>true if the workspace could be locked, false otherwise.</returns>
        public bool LockWorkspace(long workspaceId)
        {
            return manageLockForWorkspace(workspaceId, true);
        }

        /// <summary>
        ///     Unlocks the specified workspace.
        /// </summary>
        /// <param name="workspaceId">The workspace ID.</param>
        /// <returns>true if the workspace could be unlocked, false otherwise.</returns>
        public bool UnlockWorkspace(long workspaceId)
        {
            return manageLockForWorkspace(workspaceId, false);
        }

        private bool manageLockForWorkspace(long workspaceId, bool toBeLocked)
        {
            if (workspaceId <= 0) throw new ArgumentException("The workspace ID must be a positive integer.");

            using (var httpClient = createHttpClient())
            {
                try
                {
                    var httpMethod = toBeLocked ? "PUT" : "DELETE";
                    var path = WorkspacePath + workspaceId + "/lock?user=" + getUser() + "&agent=" + getAgentName();
                    AddHeaders(httpClient, httpMethod, path, "", "");

                    Task<HttpResponseMessage> response;

                    if (toBeLocked)
                    {
                        HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
                        response = httpClient.PutAsync(Url + path, content);
                    }
                    else
                    {
                        response = httpClient.DeleteAsync(Url + path);
                    }

                    var json = response.Result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(json);
                    var apiResponse = ApiResponse.Parse(json);

                    if (response.Result.StatusCode == HttpStatusCode.OK)
                        return apiResponse.Success;
                    throw new StructurizrClientException(apiResponse.Message);
                }
                catch (Exception e)
                {
                    throw new StructurizrClientException("There was an error putting the workspace: " + e.Message, e);
                }
            }
        }

        /// <summary>
        ///     Gets the workspace with the given ID.
        /// </summary>
        /// <param name="workspaceId">The workspace ID.</param>
        /// <returns>A Workspace object.</returns>
        public Workspace GetWorkspace(long workspaceId)
        {
            if (workspaceId <= 0) throw new ArgumentException("The workspace ID must be a positive integer.");

            using (var httpClient = createHttpClient())
            {
                var httpMethod = "GET";
                var path = WorkspacePath + workspaceId;

                AddHeaders(httpClient, httpMethod, new Uri(Url + path).AbsolutePath, "", "");

                var response = httpClient.GetAsync(Url + path);
                if (response.Result.StatusCode != HttpStatusCode.OK)
                {
                    var jsonResponse = response.Result.Content.ReadAsStringAsync().Result;
                    var apiResponse = ApiResponse.Parse(jsonResponse);
                    throw new StructurizrClientException(apiResponse.Message);
                }

                var json = response.Result.Content.ReadAsStringAsync().Result;
                ArchiveWorkspace(workspaceId, json);

                if (EncryptionStrategy == null) return new JsonReader().Read(new StringReader(json));

                var encryptedWorkspace = new EncryptedJsonReader().Read(new StringReader(json));
                if (encryptedWorkspace.EncryptionStrategy != null)
                {
                    encryptedWorkspace.EncryptionStrategy.Passphrase = EncryptionStrategy.Passphrase;
                    return encryptedWorkspace.Workspace;
                }

                // this workspace isn't encrypted, even though the client has an encryption strategy set
                return new JsonReader().Read(new StringReader(json));
            }
        }

        /// <summary>
        ///     Updates the given workspace.
        /// </summary>
        /// <param name="workspaceId">The workspace ID.</param>
        /// <param name="workspace">The workspace to be updated.</param>
        public void PutWorkspace(long workspaceId, Workspace workspace)
        {
            if (workspace == null)
                throw new ArgumentException("The workspace must not be null.");
            if (workspaceId <= 0) throw new ArgumentException("The workspace ID must be a positive integer.");

            if (MergeFromRemote)
            {
                var remoteWorkspace = GetWorkspace(workspaceId);
                if (remoteWorkspace != null)
                {
                    workspace.Views.CopyLayoutInformationFrom(remoteWorkspace.Views);
                    workspace.Views.Configuration.CopyConfigurationFrom(remoteWorkspace.Views.Configuration);
                }
            }

            workspace.Id = workspaceId;
            workspace.LastModifiedDate = DateTime.UtcNow;
            workspace.LastModifiedAgent = getAgentName();
            workspace.LastModifiedUser = getUser();

            using (var httpClient = createHttpClient())
            {
                try
                {
                    var httpMethod = "PUT";
                    var path = WorkspacePath + workspaceId;
                    var workspaceAsJson = "";

                    using (var stringWriter = new StringWriter())
                    {
                        if (EncryptionStrategy == null)
                        {
                            var jsonWriter = new JsonWriter(false);
                            jsonWriter.Write(workspace, stringWriter);
                        }
                        else
                        {
                            var encryptedWorkspace = new EncryptedWorkspace(workspace, EncryptionStrategy);
                            var jsonWriter = new EncryptedJsonWriter(false);
                            jsonWriter.Write(encryptedWorkspace, stringWriter);
                        }

                        stringWriter.Flush();
                        workspaceAsJson = stringWriter.ToString();
                        Console.WriteLine(workspaceAsJson);
                    }

                    AddHeaders(httpClient, httpMethod, new Uri(Url + path).AbsolutePath, workspaceAsJson,
                        "application/json; charset=UTF-8");

                    HttpContent content = new StringContent(workspaceAsJson, Encoding.UTF8, "application/json");
                    content.Headers.ContentType.CharSet = "UTF-8";
                    var contentMd5 = new Md5Digest().Generate(workspaceAsJson);
                    var contentMd5Base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(contentMd5));
                    content.Headers.ContentMD5 = Encoding.UTF8.GetBytes(contentMd5);

                    var response = httpClient.PutAsync(Url + path, content);
                    var responseContent = response.Result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(responseContent);

                    if (response.Result.StatusCode != HttpStatusCode.OK)
                    {
                        var apiResponse = ApiResponse.Parse(responseContent);
                        throw new StructurizrClientException(apiResponse.Message);
                    }
                }
                catch (Exception e)
                {
                    throw new StructurizrClientException("There was an error putting the workspace: " + e.Message, e);
                }
            }
        }

        protected virtual HttpClient createHttpClient()
        {
            return new HttpClient();
        }

        private void AddHeaders(HttpClient httpClient, string httpMethod, string path, string content,
            string contentType)
        {
            var contentMd5 = new Md5Digest().Generate(content);
            var nonce = "" + getCurrentTimeInMilliseconds();

            var hmac = new HashBasedMessageAuthenticationCode(ApiSecret);
            var hmacContent = new HmacContent(httpMethod, path, contentMd5, contentType, nonce);
            var authorizationHeader =
                new HmacAuthorizationHeader(ApiKey, hmac.Generate(hmacContent.ToString())).ToString();

            httpClient.DefaultRequestHeaders.Add(HttpHeaders.XAuthorization, authorizationHeader);
            httpClient.DefaultRequestHeaders.Add(HttpHeaders.UserAgent, getAgentName());
            httpClient.DefaultRequestHeaders.Add(HttpHeaders.Nonce, nonce);
        }

        private string getAgentName()
        {
            return "structurizr-dotnet/" + _version;
        }

        private string getUser()
        {
            return Environment.UserName ?? Environment.GetEnvironmentVariable("USERNAME") ??
                Environment.GetEnvironmentVariable("USER");
        }

        private long getCurrentTimeInMilliseconds()
        {
            var Jan1st1970Utc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long) (DateTime.UtcNow - Jan1st1970Utc).TotalMilliseconds;
        }

        private void ArchiveWorkspace(long workspaceId, string workspaceAsJson)
        {
            if (WorkspaceArchiveLocation != null)
                File.WriteAllText(CreateArchiveFileName(workspaceId), workspaceAsJson);
        }

        private string CreateArchiveFileName(long workspaceId)
        {
            return Path.Combine(
                WorkspaceArchiveLocation.FullName,
                "structurizr-" + workspaceId + "-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".json");
        }
    }
}