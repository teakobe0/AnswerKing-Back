using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JzAPI.zfb
{
    /// <summary>
    /// 支付宝支付配置文件
    /// </summary>
    public class ZfbConfig
    {
        /// <summary>
        /// 沙箱测试数据
        /// </summary>
        // 应用ID,您的APPID
        public static string AppId = "2016092600601125";
       
        // 支付宝网关
        public static string Gatewayurl = "https://openapi.alipaydev.com/gateway.do";

        // 商户私钥，您的原始格式RSA私钥
        public static string PrivateKey = "MIIEpgIBAAKCAQEA1fqXV4tTy7FcH3u+J46eOruVWxxOrawZFKYahCfle1424XjsYIk53omtUHbXSaiBzjWZ/mZSZ/QBkr4EWvrr1jDDrwuHXSDNIeK3IZfrg+gSpNomdxiTNjWvr2fRnFnUEBO2IM6Cs5Olt82pAqqjfmGAkyKGdOTMcpzk9ZfoMJBTXnqjpH08UZpX2u+lsvu8oXwCh0Ju4S3/dZzias6BY7CNOjq9GOxkC22xsjoJZgD2waUY/Sfa01+mbqprT/59MTyCn8fbCXK/wKqtVF5dexXGi44IIBGuH2CnuBEDzRAuhlCCeUQIIyCdfxZSh3HNoBqfgrJYl6Cp0ZWpluYRUwIDAQABAoIBAQCGyGcGFNP+jURq7GYVwqb2dewZTNZeXPYbZJl1PzA0ql7FXYb/M9EI1Q4fjgwD8Kl6+5Z5gXhM1I0dIqXZrQ5ah7LrEsb8KUotGwKhnXETUF5WCreJ8yffUKfORAXrh0WlrtTdC1eWx9ztzxSzgkqjs8TqaSfqf6gsVZl+/WWbGFaVWU/MYfehi0akVplR4dgYeaCfca/miZi7+9XOHfEuTcEiiap+/h/3IkEFGbHt5u2TGySHZzs2X8hJ8ANDG9AYRXkzhv1oR+SuCQ5Q/T0ZNSe/uSPLLgxuXU1V8L/yC5ierBBlZpx6Zb2k3ll5GSLVWgpR95qMaV+/u6gzwnGxAoGBAPi3jD3n1F48UpwUmC/VNvvcHJrPmVTAFJ6neKfpXxzdisT/Vbs1faNL3x4hNyqB6TArWLvwYBb1v6IvZSDYTYT74Hu59RU+GvweASVLZmeWLskKWGKlazZFCRhgv0YKgL7iTMED3ZjjGkHeIQ2rRRnM8+/MEltE14PXWv+nTZV5AoGBANw+owGOI2BHQgPAKEhDsAQCwxX58G9EVRiPwHyNAi2POhIGYEp/G9WSKuNSCarO1EOrTEj9v+dzqWnu6TrYcFmGx8RjQgOFeEQXdgbNxD8a6KJG1IxBiuSZHHTMLyTz26pw0WAGiA2YunJClDo0Cdna32Wpk+MtnXIyvXB4niYrAoGBAKkR7I1weFaO3wEmpsVtRPE0kWVTVcHcyKeI5GYQKA2xM9HyWE7ob71/p+4NWjjVErUh4jHvlwJEj74qRa5sG1YY+mJg6I9GJoXXx6OYe8ZSuv8Cv05fcF/10db2fU+ZeCdKIi7a99okFwdLfmCSWmjNf800hg3w4CfJTxyWmBDRAoGBANbeIOdGhUrLmiLQmDwQFSuEilDpW2Eh+CcTL4YTh1ltTwkAwXZMVZphlIfbpGTIjZtLBgx7ynoCg/g+RbnMKKee70uwTlnjH7bGcLYojtnF5clTcs82rktg+LsV7LSPV/ILSKj3Fkq1gsgwHi4+UJICj0m+S/IpsV4VFG4irp+rAoGBAMYWcx5gOCSGS1nBK+nugWe/VzEMmPwNanfPMx/Bxe27NCzH6LCm4hPgI6frJRe+WQaN6wlWdDwSXtbz8SBfpIb7zoxhtfRmj7+l4lPhnUWWVxAhsK/yUpyCrLNroeuQ55+w5MX/U8fKqdVR15wcOmijSLst02OCDS8E+N7ymuTB";
        // 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
        public static string AlipayPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAtukZeFE0weIewUZo3phUgPg3pPuw1GHtWerLf3Xz7IJXSrLlF76lLvsqL9xqVbRTh3BXvorK9xltaTug1DV67+OpfLFNWim2j6n7Lf3X2gHcOPL47JZKAnc/KcmmrAeU6mIszEzST6wl/GCztpDozrh02oOPQTH7LUG+xvHcMdWWuX9O1r8cG+3qL7APRFFyX2iqRcrT4+dVjvx7LEb/hAPe1bbWM5P00aBvEPetUIgyvt2+/1x+ylzGH2JzfT6p9f2JQyTZTgXpfAcO72LyNtxyOTk3n6xCt1KiJ1bl8KioLk7pUv6hCAyW7fbo8YvI8sdjFHcN346EiXYXUOzJGQIDAQAB";

        // 签名方式
        public static string SignType = "RSA2";

        // 编码格式
        public static string CharSet = "UTF-8";
    }
}
