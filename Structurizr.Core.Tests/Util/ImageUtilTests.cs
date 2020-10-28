﻿using System;
using System.IO;
using Structurizr.Util;
using Xunit;

namespace Structurizr.Core.Tests.Util
{
    public class ImageUtilTests
    {
        [Fact]
        public void Test_GetContentType_ThrowsAnException_WhenANullFileIsSpecified()
        {
            try
            {
                ImageUtils.GetContentType(null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("A file must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_GetContentType_ThrowsAnException_WhenAFileIsSpecifiedButItIsNotAFile()
        {
            try
            {
                ImageUtils.GetContentType(new FileInfo("Util"));
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.True(ae.Message.EndsWith("Util is not a file."));
            }
        }

        [Fact]
        public void Test_GetContentType_ThrowsAnException_WhenAFileIsSpecifiedButItIsNotAnImage()
        {
            try
            {
                ImageUtils.GetContentType(new FileInfo("Util" + Path.DirectorySeparatorChar + "readme.txt"));
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.True(ae.Message.EndsWith("readme.txt is not a supported image file."));
            }
        }

        [Fact]
        public void Test_GetContentType_ThrowsAnException_WhenAFileIsSpecifiedButItDoesNotExist()
        {
            try
            {
                ImageUtils.GetContentType(new FileInfo("foo.xml"));
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.True(ae.Message.EndsWith("foo.xml does not exist."));
            }
        }

        [Fact]
        public void Test_GetContentType_ReturnsTheContentType_WhenAFileIsSpecified()
        {
            var contentType =
                ImageUtils.GetContentType(new FileInfo("Util" + Path.DirectorySeparatorChar + "structurizr-logo.png"));
            Assert.Equal("image/png", contentType);
        }

        [Fact]
        public void Test_GetImageAsBase64_ThrowsAnException_WhenANullFileIsSpecified()
        {
            try
            {
                ImageUtils.GetImageAsBase64(null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("A file must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_GetImageAsBase64_ThrowsAnException_WhenAFileIsSpecifiedButItIsNotAFile()
        {
            try
            {
                ImageUtils.GetImageAsBase64(new FileInfo("Util"));
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.True(ae.Message.EndsWith("Util is not a file."));
            }
        }

        [Fact]
        public void Test_GetImageAsBase64_ThrowsAnException_WhenAFileIsSpecifiedButItIsNotAnImage()
        {
            try
            {
                ImageUtils.GetImageAsBase64(new FileInfo("Util" + Path.DirectorySeparatorChar + "readme.txt"));
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.True(ae.Message.EndsWith("readme.txt is not a supported image file."));
            }
        }

        [Fact]
        public void Test_GetImageAsBase64_ThrowsAnException_WhenAFileIsSpecifiedButItDoesNotExist()
        {
            try
            {
                ImageUtils.GetImageAsBase64(new FileInfo("foo.xml"));
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.True(ae.Message.EndsWith("foo.xml does not exist."));
            }
        }

        [Fact]
        public void Test_GetImageAsBase64_ReturnsTheContentType_WhenAFileIsSpecified()
        {
            var contentType =
                ImageUtils.GetImageAsBase64(new FileInfo("Util" + Path.DirectorySeparatorChar +
                                                         "structurizr-logo.png"));
            Assert.Equal(
                "iVBORw0KGgoAAAANSUhEUgAAAMQAAADECAYAAADApo5rAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAACXBIWXMAAAsTAAALEwEAmpwYAAABWWlUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iWE1QIENvcmUgNS40LjAiPgogICA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPgogICAgICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIgogICAgICAgICAgICB4bWxuczp0aWZmPSJodHRwOi8vbnMuYWRvYmUuY29tL3RpZmYvMS4wLyI+CiAgICAgICAgIDx0aWZmOk9yaWVudGF0aW9uPjE8L3RpZmY6T3JpZW50YXRpb24+CiAgICAgIDwvcmRmOkRlc2NyaXB0aW9uPgogICA8L3JkZjpSREY+CjwveDp4bXBtZXRhPgpMwidZAAAl70lEQVR4Ae19eZAc133er3uunb3vBZbEsbhFECAp0KQi6qASuUSdcUpCrFhRmVaKRZct2WGpKin/RTGVip1UuRjbtCKKFZmuRIkkSJEtyYpsMw4PuRjSJEVABLVLgFwABBbYE3vv3J3ve9096J1dENPTs7Mzve8BO/369et3fL/f937v6m4R7TQCGgGNgEZAI6AR0AhoBDQCGgGNgEZAI6AR0AhoBDQCGgGNgEZAI6AR0AhoBDQCGgGNgEZAI6AR0AhoBDYLAcuyjM3KW+erEagnBMgFs54KpMuiEdhsBDQhNlsCOv+6QkAToq7EoQuz2QhoQmy2BHT+dYWAJkRdiUMXZrMR0ITYbAno/OsKAU2IuhKHLsxmIxCtVgHc9YxHHnnEePjhh+XEiRNGX1+fce+998rLL79sJJNJ4/z5OI4XjUQiYcTjcSMWixkzMzPq2NvbK4uLi+bCwoIRjUaNjo4OiUQiBsIMHtPptCIv/aZpFv9SqZTy4zLSFnGvIb7R1NQkmUzGyOVyJvKE30DcLPNX4YzL+mezWYMOZVJ+hvEag+k3jKy6Rn8u54blnGv2uRvOOHSGYV+3z3hux3PP/R6Br+W9x7Kiq86jUfu6G8+97oYDB7GsmLonFrOsQqHg+GOWfQ1XcYF58BqwsIAh/DH4LeWHXAoMB+YCDFUajLuyIjgvqHNgXgzntXw+r9JE/AL9ra2tKmxubg5Y5qy2tjaGFaampoi91d3dbfGIMjFPpH2ztWtXBscV69ixY9bTTz8tk5OT1vHjxy3omkDXVPqAVx29GFXiN6jI5SQ2MTHROjE19Yk3z45+6tTp4Z2jo+d6L10e7/nrk6MdcmE0JjJVSf76Ho1AlRDoFdk5lP3I0d1zNw1umx4a2j119PChC3v3Df2gv7f3R/39/Ys3ykhx4UaEwPX4j3/y1O997y9++JVvPP4k0pwvSfcmkTia5mYYmwgacbbj120M2ahWhcglZaj0tN7KU2k9tuB9So3wU0Dd8/hZzolkYKrkUgkY7fKFB++XT//KJ7/ysfs+/Pto/DMlEYqnLiFMRGKya9xLL5068tjjTzz75BN/0qkudhyUm/rbbIWHhabRzeMHCa1Wc9t6r0lPB2gEqosAGjTVcbUP0GO0yYaojjAbZejhpckFkdkRle2vP/Cl2S89+MAH7rzz6M/XKwf02GSXaQ0hyJT/8e3v/ebnPvuZr/LGoXfdrYg4u5SWDNiYRbewQBKAEKuooFi7XlY6TCOwgQg4pGAOBv/hHEotsYghcfRaOlsSquMy+osXVCG++a3v/tav/eqnv1Y6VCAX1gyqSYY//urX//B3f/vBhyS6Vw7dul3euDyLwZUlBqkHpWeGdDyyAEXn8RbDtEcjsAkIsMFOZfGXK8jcShaTJIYcuv19MvzaZWFDPzX9+H7o+pdLSUFCFNWYZPjaE3/2xyDDFwf3vluisZgMj06J2RRVCap6FWNvQi11lhoBHwgUG27VhxKlyzv29Uo+2yFs8OOcVrOs3/GQAnNVmJ9DAEYkIj/68U/+9Sc+dt+jPTtvwzRaXMYmFyXaFFFjBR/l0FE1AnWJAHmRS+VlsK9V0umMTF84SZ1/CDr/n1lgcoFzQhGenDlz5vA//+LDj4pEpaOtGWRY0GQgMNqFBgFOArGBp25Tx0Vi8tkvPfwodd+pJOdJhWYjcuL7P3p2+a0X5JY7/pG8NTYrEXSTmIB2GoEwIUCdpm5Tx2+54z2y+OYLQt0nB1DPGLtMA6dOvf7+2247fELaD0h3d4vMLGWujRnChIaui0bAQYCTRN0tcZm5uiwyNyInT54+fvToLc+ZWIB46O9fePFJxju4u09mFtLYMqFHzg5u+hBSBKjj1PWDu7DCDUcOkAvmwtz8v/2/z/y0hYFLqSwncNX6As+10wiEFQGuoVHXlc7D+3fgALlgDI+csQ4d/DiCstK1q1NmMWfrTleFFQxdL40AESApOpMxuXr+Ks7iMjzyV2K+fZF7P96Q7Xu7sIiR96xK8BbtNAIhRgAjA+r89r3dqOQb8valS2JenZlRNY7Hompfkh49hFgBdNVWIUBd51486j4duWCeHnlTnSylc3qaVSGhf7YSApyGpe7TvQ4umKffeAveLlnJ5rlSpy7oH43AVkGAm1Op++TA6ZG3xLw4No6TTsmCKpoOW0UNdD1dBGgDqPvkwNtjE2JOzMyJtMfVblY3kj5qBLYSAlykk/aYTF6dFXN+ESt1MXP1cw1bCQ1d1y2PgOobxSIyv7gi5gp2/eHJe70Yt+XVYusCYC/SmbKcSouZzWGEredat6426JorBLgYTS6YeTwSiuchNCwagS2NADmA1+aIqadat7Qe6Mp7EODgms9DaKcR0Ag4CGhCaFXQCHgQ0ITwgKG9GgFNCK0DGgEPApoQHjC0VyOgCaF1QCPgQUATwgOG9moENCG0DmgEPAhoQnjA0F6NgCaE1gGNgAeBNW//9lzbIl7shefzIXRqS5d3X5fH7/HakfVvYARc3FVC7okrD54DdIV77cDfooRwwDdgIA1AwK8CcIMj9wGrvcCe4yphBVYBnYAXAa+yqw2mCPAeC3i0k38W/yATb3xvOlX0bzFCkAggQSQOcHHMp0XS82ItXcDnmOwHzVdhy+jabTwCStFLsmFYc58YyR68oZhf08SrVwuQEf820G0tQpAIVkGsuVG8agEvp0pExOw5Kua2TwL4DjHjzWLgjwIwonFwhtYDgiB5NsF8b6DcNzlptDSuJcaWa0tZgqxYubRYmWWxsvjLLElhYUKsmbNiTQ0jPiTQvVekqQN+x3JsQC22ACGApPpQEp4KnDsHi3BVIvs+LtHtRyTauUNMEMFItIEACfu5EMMhAE03/tQXkpQZ3wD0t3qSihTUdYcgaKzYYKlGCyQhOQrpBckvTkpu+i3JXXhRrLFXxOjdjxft4VuHebx6VX3sbT0TUxm4hjlwK0oTVoeq0SpkV8SaHpbIwU9JYv+HJNqzR8xEq20BqOwQTFEoSjjEw4XFPYYVI09Vq6dXPsDyZKoaHp7jT/3nD981DJLkMpJfmpLM2CnJnP6BWPNnxei6RckOpsW+x0eu14sabkKADNbSuBg4Jt79eUnsvBOfB+uwVV0N1NgaERpX6SkMxxW9RY97RR+rhoCLu5Ng8bTowQWbFOy2Gvhj9yo3d1FSw38juVP/HdbiVrsHQGtRBUseUkIA0EhCjRXM3lskeefnJdZ/0AaMgzIXb6XrWuGrpt8blhAEVpQZOrGRmOpKpc4+K+nn/1CMjgN2T6AKpAjhGMIhw/zbYvbfJs3v+YLEundjHIYWBAM41YpUoSXZMNnrhNdBgFbCCWb3FoNvI94iyUO/jGNSUs/8ezE6D9mWImD3idMn4XJmTGRlRoz2HdJ816+DDEMAEB+v5wDOnS0KV423Vm3YmFGOyhpEpGnP+yVxz78Ra/I1u7EL+FR0uAhBoNAlstIzkjz2LyXWuxeWAWsNdNoq2DiE5deRNafFm/a8T2Lv/g2Q4lVMmXNq3e1f+a9suAiB6VVr6jWJ3/Z5iQ8ewQDMGS9oMvjXjEa4w7EUnDFsOvBhrCf9EtaXJgORIjyE4AIaVp0NjBsSQ+9V6wpq2V+ToRFUO0AZMS2L7lOkY1Dihz6KiZQL9gCcZKnAVXZXBRlt7C0wkbQO02clduCXJdK+zbYOxZHYxuauU99EBNjgYZ3CQIMY236rmLvvFWvxMmadMJYsTk2VX75wEIKry9kUBtIDEh94F6blMHmmZpTKB0LHbGQEaCVyEmnpkejNd4gsXkFlOI5wp6bKr1vjE4IDKFqHxYti3nSXmC29GFO5gyr/gJQPnY5ZNwi4VgJWIYopdmnGN+MwNav2ofm0Eo1PCEqF44flOYl07VYb9ILMMtSNkHVBfCJgN4KR1j4xO4fQY1iyp2d9ptL4hGDrwNkkHMy2fvygu8S9L9ptMQTQbYLcTSzYGZ07sTEQHwJSzl8vIQSEQBXQf5R4FN2lHjW4UhZCzy5tMUKguhxcY/u+CSsh6dlKxtQBl/U2HXJn4FTAx+abb4ZxSGx6iXQBNgkBGgKqA7rP3NZhO0c/nLNyDo2/l4mWII+vICW71KavcipdT3Hsnu+1Eqlzb6Ar6FLL78rajevGY1Ll+K9leW0yxk3Le62c9JyylRax9Lw02eqfswKmmDE8YacwwLk6lp9TYxOC9ccCDLdn8CEf9eyDmmFSF8pHoUYxWSr+sZ9KOZn44QNI9tEOQ3Dj7TJBpdy6qSN+8BycHab8rNW1+tlnG/PLD5+opx5Vl5ml8ceIxiaEwhQVzq1gZRqPfXJAXYeOYiEJ4tB8zIdJAaTNIpCfg01jvSTFCCSyOsJPD/2N4qhzVDwe8ReBMiZR1xj8URybcM7qZFHHvFM1Rq2uY4rMhQVA15kvjyCmihjl51SfGlR++e0K51ds61CHhGDrH4NQclD+8TQG/zkKDY4MiUakIxaVfmhOMmIqwlCBeI+yIj6FqdKt4Q9rwjUfftU2j2MOk3sk+CLqOIWPoS9mEZDnGzMQEXVqjkekPYJPV+GUDULVncrHfl4C/WckzwAiWb5rfEKwroUUCIGq4Guq9eBcMbCFTEFbpkkEaPmBzibZ0dUs3e1N0tmakEQihmJHJMo/ECJCMuBPmX1U5BofkJASbmntvOH003k1zXvdvlrer3tf6XH13WyA6VxS8JNUeRAiC2bkcnlM/uVkcTkjVxdSMjm7IqdnVuTKMp5LiRoyAHKQGG77oBKq1g8bRrWXyYtFeYk3OCFYYQhNmUY+YkgBbq5jiSBvlkrGU2gdo6b8k309sm9Ht3R34KGWJFZTI3jbh6P4LLt/sW1uHW+Uu5KC80OS8GOGabzm571LKbk8MS+vjk7LL6aWpSluSgfwyQAAR5I3SvoG151UVMOIAvCUbSSPZboGJwRqSeAVDqqTUWa1NyYaixFHeWgV5jN5ec9NHXLs0DYZ6OuQOD4MTmfhGlvUPFpRH3JS9zbij7J2aKiSTTFpTsalr6ddhnb2ypFzk/K3w+NoNHIygNcBsQtFi6F4FLiiri64CPNYXsqNTwiCp+rNCpdXad5SbcciJCCHGfSbKdzP3H6THN6/HUoQQzeaBMBrVTyZut0iT1AovSQ//1zHere3NMkdh3fI9v52ee7Vt+WVKwvSn4yqATdsanApBlCDcBCCaBdBuAa+K4SNPjLHBPKfBhlMjAW+cOdO2TfUr4qUxeCSbqsQQFXW88N6ex3JkcNAm+GDA53y0fcmpO1n5+SZ0auKFOROUAlyKrtSR9sSMlc5GJUAQeFx8LwIK5CDkD//S7vkwJ5tkCqsQh5bCRBWqhSV5BOWe1w8SIxsNiftrU3ywTv3yAd2d8nESg4zbQFrGlD8QbMPWPrGvp1kwCyimnZcgnX4F0cHZd/ufjyol8dA0tJEeAfxusTIoivZgrHFPXfskiN9LXIFExEJTDhUbCUqvtEurCbEOwjtRpfYGLHPOY2B4YeGuuQQxgw0+Fx401bhRujZ14kTSdHZ3iwfvH0HzK0pS5hw4ExdRbqtLUR5wFc7FoVFoc1AeL0tMbn90HZMI2Jg6HSTqp1f2NPLw6pux5jin+7vkwWs23AxsyJXEYuu5aQtxDUsfPkIHEWWRVfpnqEe6etuU4tRntG9r/S2cmRaCXYxuTB5YKhPdnY0yWVMW3Oiwrd+V8gjF/9wESIgGC4o5RwhOzWQbse06p4dPUqYHChW2rCVk2fY43ABj12n23d0cqMXFvdrKFAH3HARwndzUrmKEbhlCO1wb7MSIls47SpHwLUS3MYyiPUJjiWW0f30PZYIKIZwEaJyefi+U7VdIMHNva0S474cWAftgiNAK0srcaAtIXNocKJ+rURAoxIuQgQEo1xxMhtuY0Y/CcJLKtNud5dqVIByC9qA8YgjNz32gRCcz/aNaMB2KVyECAhGufrD8cMShNWJvRptLfqx1XJxKyce93rFYXF7SQjIU01hl3OjG8c3g9wb7WO4CBEQjNXQXP+M2axAcF2YZm1Ca6ZddRDgOIIWIoLdqlyso3mo3oa/8soYLkKUV+fAsRTvQIjWJrzpA8KjELWrDgIukjE0NuyS5oGz32FEkJKEixAumkEQucG9zIItGfcqtcC0czOfdtVHQG2XR9+UDxCxAaqBaFUltDQrkKWyEJBQEx5u4WJSzaRVQVkb8RY2ODFMv2LmVfidUYV3jSoSLkLUCDmbEBj8gRBcPKpV61UjnaiLbPhIbRLY5mCJfYk1oDDCRYiAYPjSBOTFFwKo7pOvG3XkchCg5eW2ejyN7g9jX+xZW5JwEWJt/TYkxLYQ3PpNQjCLWjJxQ6pUP4k6UPKZc+Lre74ioCjCRYiArYNfrVDWocZ5+i1jo8YnrBUpZ0B5VJRn3YIcsHXwWy9ah4D4+80y9PFdEXqxrSXG4SJErdTFkZB9qKW4alXBesgnyJPRlZdfE6Jy7K7d6TZr10K0r0ER0ISohuC0kagGinWRRrgIoRWzLpSqKoWoVJYBrXW4CFEVSehE6gKBShW7UiI5lQ4XISoFsS40QBeiHhAIFyECtg71IBBdBgeBTZJluAihLUR4+LRJsgwXIcKjDromlSIQkEjhIsQmmdlKZafv2wAEAupAuAgRsHXYAPHoJBsMgXARImDr0GCy08VdD4GAjWK4CBEQjPXw1WENhkDARjFchGgw2eni1h8C4SJEwNah/sSzhUtUqSwD9hLCRYiAYGxh9au/qlcqy0qJ5CAQLkIEBKP+tEKXqNYIhIsQlbYqtUZd57dxCATUgXARYuNg1ik3CgIBewnhIkRAMBpF5rqcG4dAuAgR0FxWDPNm5VtxgUN8Y0BZhIsQmyJnSEBbpg1A3tqUt12FixC1UkynFQrYGG2AEukkgzZO4SJEjTWUb5Xz/WY5rbPviIDbphFXfhuCrpZiDRchbPxq8wvJ8es2tRRWbSpWH7kQ2834bp8mRAXyd0mQw8c87I+luO1aBYnpW1Yj4EBJMuRxhV8bqKXThKgAbUUIvGsxhy8v0rRrV30E8sA2DWzxHSGn0Skzj4Dy0IQoE+c10dBypfEdZX6fusaN2JqihDEgC2yXgS0/y+tLxwMKQxOiAm1SAoKgVjJ5fAMNQ7+AQqigCKG+hd3QTDYv/CIpvxHhixABkdGE8AkgdV+NG4DcYiYnBbRkmg8+QSwjehaE4HeqlYUAI8rGOCB7NCHKEE5pFGLOD3ospnOSz0NwfHe7dlVBgN/cIL6ZNL4uh3EEB9W+dDygKMJFiIBglCtRzo+3QlJj6bwsrfCzgNpVAwFaXrYtuVxe5pYzKsmIX0IELEi4COGrKakcOc4sNYEQhUxBFhZTKiFtJCrH03unaZiSQVd0ah64Qjv5lQhfYvUV2Zuz7Q8XIdbWb0NCiLkiAJgxNbuMbhPHEfweWkBpbEhpGytR4roE6zC2kFYfbndXq2tVi3ARQnWZaqOUGO/xM6QyOrkoK+jvGuzsahcIASJIWGdmF2VsKSt9wDcLoH0h6yvy2uKGhxBEUnEhICJrMVo3hC1XV9SQ01dXZGJqAYPs8EC5boU3OJDW1cS3qVOYqBgdm+NUnvrssW8LoXSg8sKGQ4qKA0VGVI6GjztpIWK0CpgJeX10EoLMSgSk0N0mHyCWRDXRXxqfmpNnLs1LRyIi2Qq7oEF2mIWDEAS2QvBKZFL2KemnrETclJ9enJfzl2ZUC1d2AjpiEQE2IlFYh+VUVl59Y1wEaxDNOM8B5IrsvdIFSsj/3Q1OCFSYlWe9LahnDUnBLCmwuBo7WPLc6TGZnl2SWCyqtnMUpa0974iA6irBMnCkMPLWuDz/9pz0J6OS8jt2ULkoRbB1wc2VvPBBjAYnhFNrMwYQsNRPUtTQEf4MAB+IR+TMzIq8cPKCLK9kQIqIJkUZciAZOKsUiUZgYaflOz+/LC3Akjqs9LiMNNaNAl1QDSQF5NOFgBCALpJEXx4LZAVuGK6to+Cy+GGr9sy5q/LiyfNqYBgHKShw/mm3FgHXMkSjUbl4eUb+8sVzahtMK1biiGcFuuxkgpvzOfjZODIVf/hzd21jOypcpEmsHOatCwSito6QE3qMraUPpPjfI5PoAhfk7qM7pb0tqbZ2cEcsHbclbHXnNhBRWAU+8/Dm+Un5y5fOy/hKTgYwkE5X1FUiqsDY0X8rj1Vui7rgH+/GJoSqr00IyS7DSsJK+McgsI4yS2WbUBRaiqfOTsvUYlred+Rm2d7focYV3PPkEsPNMOwEcZXfW99oJKJktLicluE3x+W7r12B8loBycAcbMGz22xlscrNblMFDVBjE4I4AEwjCguRmkWjwP0vBKb2rHBJQYO1rTkqr04syelnz8p9+3pk/64+6epskTgG3HRUFJLjmsKE50EjhbyjiJxG5YIlj1RO1nkplZHLV2bllTMT8sqVBWnDmKE5ZgawDApSooo/5g4s0Tjapzjn9nwfrsEJgQpzIM1B9fI4GoXad5m8WNviEMyQgBRNUZnPFeSHr43L4PmrcmSwXQb72qSro1mSyYQ0JaJq3YIyNLB/h/euZzEc3fJmwzbApjxvcp2rD+655/gOl4qxmKYqSzFktYeX6dw6KoWzg4q/Kg4SYlr4xY8lKexLSmM6dWEpJZMzizJ6ZV7+YXwJigqrAGtKdU3jx1uVYoK+PUiF2WZW7H4sN0PZtrvslBqcEKgn0Y9EYSGmxEov4pzoumKrDsxlo+lEZK6cNmzBALG9JSZjqZyMjUyJoCvVl4zJYGtcelvikmyKSSIexV9E4uhTcy4+whZVtap2YtwjFcwBnzVprBfmLxel8LiF8KsXAqC+fMac4yc+3MMVZ27hnsdu4CuLGTm/lJEcHqhiUTpR3yS6TsQoeEmccjMh4GblUlJIYaUb2z7WVLuMKjY+IdjGmAksCqAtWISV4EwTCQGwKwGkDMzKikI15mxJDhozgMU7A38pjLynoCiT3DI+DvLSsayM7DqvX4WtCXBjVvnoRzUZl45lc/0erxtEtrB+UNQmNA5dsAgRnBOTFciHd1evdkgPOwUK6SUpzF0SSQwgdToWpvxcGp8QLvhoEPLzV9BtyogRwzSsMsYEZPOcKwa3SxCFYvTjj84tNnlLP7sOHFPQ715zjwiqgXNLW05W1+K6Fowh7PKxk0I/q6nCmBw85AYXMt3tGLxWfYdxSmperLkL0IEWO1OfmTQ+IYgspluNjt1SmPiFFFbmJRprLiqVTzw2JLorfCqEO8phmBvORhRzLwiwQ9zwDSnMBiXqklcd8cNp6GKY42G9NqZuyIDYgXU5WAdr9owYvUdxzrk/fzk2PiFYYa4/NHVK4eILkp+7KNH2bY5yURL+ANkgfVHJlpakVGHc840sw0amXVo/N6/rhbvXAx9pfiIxPLC1JLmJEVvknGjJ8+Etf7nTwoXAARDM1EgiIZnzL6Ifif65iTa3wTSMomvkv81UJM7U5a5ekPzos2J0HnSsg/8SYRu/Pwb5z6IWd6AO2LphdB6S3C++LdmJYVhQEERVrcFYUQu4wpQHZxVpHTCYTp97XqzFC9hx2ar0wa91UGMgPthybYGokZFyrETzoKRe/7HkF8bFiMRVv7KRa6XL/g4IsKuk1nAMyVz+ueRO/bk9duC+Nmc89g53r7pEDkQw7W3GsLmK6YbCYeuG0TIghQv/R1ZG/hZ9SqxYovVQaxOhqKCuRBEBR2nZ6GVnzknqZ/9TjNbddle5gk2eTI5cMJubMIeP5W2fhCqWq+48nHHquU2yr3xdUmefhunEgLtIirAwv+5Qr22BqL3oDhvRhOTmx2TlZ9/CVOuoSLIb03jYvuNTmVV0cCCZiIvZ3oo5e6wsuvPJta3ZBuTGPiWc0X2rpH/6B7LyxlNqsxfBU865bp/o38ZCAESg/NDNp2XgIHr5H/6b5M/9Dabd96Lxw45nn2Rg/ZXuZwvS3tosZl93p8h8Rm0XaCxwrldaNB2chsUskyLFc38gy6e+f21MoayFA6wyu/BrV6cIUDaUlSMvqi4bNpxnxk7J0vOPgwxPoUdwu00Gmo0KnJpYAgf6uzskumN7v/w/mcMD8x3Yu6+yryDJOruFrQS7SmZUjL7bJfsSgJt+U5oO3SexgUNiNrWrAqttHspiOKBXCGid1T4ExSER2CeiguMPg13DwANXGCNy4S194SXJnkQ3CbIzeg6jmwTLUKFjDnxZREZmZcfggEQPH9wjJ+SqJGNDksbuzHDMOKGWBFM9JAIwtx2TwvgpWX777yR68LjEdhyTaNcOMZNdaHHiiIr1SU7Tuq2R8lEoJU5ZlJIwRSJHgLxEhFU8eug81+wAT5j3msdvwM/TolOJ4sybJi/i3L1UzGfVjU4KxUieNJxL6uDe46Zfeq30fsZfL4z3ueHXS8uJs+ay02n3dnmg8Gy0LGzHyC1OSXbyDcm+9RwmTf4es0nvsrfo8NkH7z1M3ofjVGsSTzcugQPkQvSWg+h7wbVgO/I8tulyb014HFCncmIHpNF2M6xGv+RG/pfkhr8j5sDdYvbuk0j7djHbBmA12tAvjeEvoY5qIM4pPfRXYWpsOSthhwedTa2JajQgGxwtbrGg4nO6FHvR+FxLgX/L01LA/jTuPihMnBbr6lm8VPcmMba/G/fhHlqGAGRg/bkM1wzdp7vlwF40lF0YmcNlsjm19dj3m9LU3fX+g1oTPI4reo7YoC+MYZn/FWxZ5qwE/sd7MEvRK0YC3SnshRKuYZAgaoGPhEAkTYjqCbpICH4HAspNMqDhsjLYZYDt29bKOMLwXANdvFuMph4x+jFWoBz4iCgtYkAysO3ndvssdJ+uqxv5DI+csQ4d/DhOs9K1q1NmsTU5YD4q8fr8IQRw6I+qP3XCVootFGYv2MXi2AODcot+p+VCgMLf/mEaJMd6R5Wg56c0Hi+5Ydfzl15nPKfcqwjpDXPLwrh03vPSeLzu5uE9Mtx1pfcw3nrOzcd7dOOVpu1NE3HcBkY1OGiouNWGXVfuQcLYT2298TZGJJBaX+As4vXK4+Zd3pFJduL5lKt4gAusk+GRv5LotoF+Of6598uJb/5XdJvukqt40ex6T26Vl0W9x3KA5ECayq6ARRiFQyFQIPjP8OpAXu941GP5oKUud+jhjKFyDKRUqicZjpdbEiCEnJPPfO5fCbkQbeto/48f+uD7vghCtLTgCS4OIgw8zKEsml2SkP66wFIAjgScQ0gr3ODVcuVVnWooAwVdVzqPJP/xB+9ZAhce45Dx0Xvuvut+ZjNybkq62xKYftWaQTy0Cy8C1HHq+sh5PNoLd8/dd99PLuBPlo4eveX7/+E/PToj8yOyrQs7BTH9Gt5xhKq//tnCCCjdho4rXZ8bEeo+OUAukBBZjBnyx//ZJz6QHLpLXv/Z87JnsBPPVuTUlNQWxk1XPYQIcJqVuk0dp64377lbqPvkALlAQtAj+/fvP/3tP/13D8E8yNzCsnplSi6V16QgONqFAgGSgTrN1wFRx6nr33nskYeo+04F8yREccDwyY9+5I/+y9e/8dj0hZNqimvntnbJ4RWD2mkEwoAAdXkHdJrTt9Rx6vrHofOeulmcZCwSAmYDM1HW72Sy2ezv/vaDD0lkjxw6MihvXJ5Vb11Tn41CbD2+8ECovXWLgKvY/AA8N/AdGuqV4dcuwzC8KX/0p48/+psP/MaXqfOeCihCeM6p7IoUX+7t6Tnzuc9+5qvDr74lQ++6W70mZXYpLRm8iCqLETpfTsXZSvvFKUjCm+yqFPWJRqAGCDizslxBYoPN12fGsHwQx8bAzpaEej3O8Ks/VQX55re++1u/9quf/loJGdQ1aj/uNbj8t8a99NKpI489/sSzTz7xJ9gjDtdxUG7qx54fZEYykBR5sI8LHGv4wAhexwKXBBUvl15TlXMC3XvcOO7Rvdk9d48Mp5/OvZd+97p7ZNg7OTeemxbjetNb7143LuO597vxSq8xfL30Su9jvPXCGE53o2uMs14+DKcrvd8955EXlRzdQIZ5nBtcevREUV5q6I30wU2j9N7Sc8ZTrugpnlEv1ZsPcbSztOTSxIIIZpLo7n/gS7NffPCBD9x559Gfq4CSH8UF/CCdVWZjVTRcj//4J0/93vf+4odf+cbjT+La/KrrIoPY+4OHjPCCX/v1gSgoS+M6j9cN0keNQMUIeMlNkvEPU6iyjLFulnufxkqSbpcvPHi/fPpXPvmVj9334d+HrnMj1LpOceFGhHDvnJiYaJ2YmvrEm2dHP3Xq9PDO0dFzvZfGrvT89cnRDnl7FEvc025UfdQIbAIC2Jy5Yyj7kduG5m4a3DY9NLR76ujhQxf27hv6QX9v74/6+/udd4dev2i+CHH9ZOwrTIy+Rx55xHj44YflxIkTRl9fn3HvvffKyy+/bCSTSeP8+TiOF41EImHE43EjFosZMzMz6tjb2yuLi4vmwsKCga/KGB0dHXgLQsRAmMFjOp02eWQefHeO+5dKpZQfl42WFk6s2dcRvxgnk8kwT8EBYVnHn1HXMX9AC6nuc4/uu3nsS1l1DeWVXM6Nl1sVn3m61+g3DPs6/XRuuvbZ6l9gsCoA5Vl17j0Bxt72EY1jtHgejV675sZzr/MaMFBJWVbMisXsuAXngxXX4uMqLjIc9bWAISZTYvBbys8wXuMf8FRHJrq0xHM7vKmpqRjHTR/fxmD8Ao+tra0Wj3Nzc8AsZ7W1tTGsMDWF5x2yWau7u9viEeVlntbKys3Wrl0ZHFesY8eOWU8//bRMTk5ax48ft6BrAl1TGADjIhZezPz4gQPHArYi+7lRx9UIhBEBcoHrENppBDQCDgKaEFoVNAIeBDQhPGBor0ZAE0LrgEbAg4AmhAcM7dUIaEJoHdAIaAQ0AhoBjYBGQCOgEdAIaAQ0AhoBjYBGQCOgEdAIaAQ0AhoBjYBGQCOgEdAIaAQ0AhoBjYBGQCOgEdAIaAQ0AhoBjUCdIfD/AS/iT1lRKAxOAAAAAElFTkSuQmCC",
                contentType);
        }

        [Fact]
        public void Test_GetImageAsDataUri_ThrowsAnException_WhenANullFileIsSpecified()
        {
            try
            {
                ImageUtils.GetImageAsDataUri(null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("A file must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_GetImageAsDataUri_ThrowsAnException_WhenAFileIsSpecifiedButItIsNotAFile()
        {
            try
            {
                ImageUtils.GetImageAsDataUri(new FileInfo("Util"));
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.True(ae.Message.EndsWith("Util is not a file."));
            }
        }

        [Fact]
        public void Test_GetImageAsDataUri_ThrowsAnException_WhenAFileIsSpecifiedButItIsNotAnImage()
        {
            try
            {
                ImageUtils.GetImageAsDataUri(new FileInfo("Util" + Path.DirectorySeparatorChar + "readme.txt"));
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.True(ae.Message.EndsWith("readme.txt is not a supported image file."));
            }
        }

        [Fact]
        public void Test_GetImageAsDataUri_ThrowsAnException_WhenAFileIsSpecifiedButItDoesNotExist()
        {
            try
            {
                ImageUtils.GetImageAsDataUri(new FileInfo("./foo.xml"));
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.True(ae.Message.EndsWith("foo.xml does not exist."));
            }
        }

        [Fact]
        public void Test_GetImageAsDataUri_ReturnsTheContentType_WhenAFileIsSpecified()
        {
            var contentType =
                ImageUtils.GetImageAsDataUri(
                    new FileInfo("Util" + Path.DirectorySeparatorChar + "structurizr-logo.png"));
            Assert.Equal(
                "data:image/png;base64,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",
                contentType);
        }
    }
}