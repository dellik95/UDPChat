using System;
using System.Security.Cryptography;
using System.Text;

namespace RSAencryprion
{
    public class RSA
    {
        private  string key = "<RSAKeyValue><Modulus>6sB5XTAvLtKgyyQxbNnPIOb3LcfCKhqUQJgQuNUCPDJ1cL3E9v3CTnP7tCmsn9iLjPoS6wMRRX+tRDP+tnKYT54IS8AUvfTOeoGJQ3ba4cbjtGtnIWZwZsvbUi5lyPaU7UBISY9RP07JJkLjJs0AjXRrwqEiSrJIlnztQPJ8OK8=</Modulus><Exponent>AQAB</Exponent><P>6yx1NTiW19Gm870WPJVp5xDM2KklJS0vI5D47HXQ7c1qjKvlLlaEIPCFONfPJ9ZiIUjk0Q50W4L7XhQ77/xNVw==</P><Q>/4p0F8hRsNy/6KkAOyZQukUn7svt6pw664yDPqkVzCcjyFpOVdPHP/4/dqnzBZ7xg4F5py/tzDkK3B1nkzOAaQ==</Q><DP>nduWxWW54x5oeZ2ICpyknKSrTBelxmGhDbenT1n6J1XGWqybxtHuGQo58qDx/aSq8/qxFR8lvbObNVhRr1JlOw==</DP><DQ>R5nIu50yXocL8qnf2bL7raWQ8dUMHc+WpsdhQt5nsCNLYGkFSAEl9CVOLPajlHCbpUhTCOhiDfXxuAk5K0Kj+Q==</DQ><InverseQ>dhHeHbOhB1H+6dh/7UOPOBSEaY4Af7vJucZq7CQFb56sY7S6G13gNG3QmlgVPR1N1qQEZCT11ghnbIk76cA2+Q==</InverseQ><D>CFeouHgS5S4VCsMRgpXG95tdo2Ha85YYOKduyLLBLpR4efY0fraL5i/W/RMSEGgopWzpqTJltbuQ08CpR6CQ6wn5VW9TgB0kkq4mrt2uJLz/6ZSQ8JOBwa+Pa4CYT0fqRxVDNvdUfgyw2xDawDGvR/GqAZ42YzgxQJsugazH4XE=</D></RSAKeyValue>";
        internal  string Decrypt(string p)
        {
            byte[] decrContent;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);
            decrContent = rsa.Decrypt(_tobyte(p), true);
            return _tostring(decrContent);
        }

        public  string _tostring(byte[] p)
        {
            return Encoding.Unicode.GetString(p);
        }

        public  byte[] _tobyte(string p)
        {
            return Encoding.Unicode.GetBytes(p);
        }

        internal  string Encrypt(string p)
        {
            byte[] encContent;

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);
            encContent = rsa.Encrypt(_tobyte(p), false);

            return _tostring(encContent);
        }
    }

}
