using Ignite; // Requires Ignite.Binary

namespace Ignite {
    public static class Cryptography {
        public static T Encrypt<T> (in string key, in object data) {
            var array = data switch {
                byte value => value.Bytes (),
                ushort value => value.Bytes (),
                uint value => value.Bytes (),
                ulong value => value.Bytes (),

                sbyte value => value.Bytes (),
                short value => value.Bytes (),
                int value => value.Bytes (),
                long value => value.Bytes (),

                nint value => value.Bytes (),
                nuint value => value.Bytes (),

                float value => value.Bytes (),
                double value => value.Bytes (),
                decimal value => value.Bytes (),

                bool value => value.Bytes (),
                char value => value.Bytes (),
                string value => value.Bytes (),

                _ => throw new System.NotSupportedException ($"The type '{typeof (T).Name}' is not supported.")
            };

            using var aes = System.Security.Cryptography.Aes.Create ();
                    aes.Key = System.Security.Cryptography.SHA256.HashData (System.Text.Encoding.UTF8.GetBytes (key));
                    aes.GenerateIV ();

            using var ms = new System.IO.MemoryStream ();
                    ms.Write (aes.IV, 0, aes.IV.Length);

            using (var cs = new System.Security.Cryptography.CryptoStream (ms, aes.CreateEncryptor (), System.Security.Cryptography.CryptoStreamMode.Write)) {
                cs.Write (array, 0, array.Length);
                cs.FlushFinalBlock ();
            }

            var name = typeof (T).Name;

            return name switch {
                "String" => (T)(object)System.Convert.ToBase64String (ms.ToArray ()),
                "Byte[]" => (T)(object)ms.ToArray (),
                _ => throw new System.InvalidOperationException ($"The type '{name}' is not implemented, only string and byte[] are supported as data types."),
            };
        }
        public static T Decrypt<T> (in string key, in object data) {
            var array = data switch {
                string value => System.Convert.FromBase64String (value),
                byte[] value => value,
                _ => throw new System.InvalidOperationException ($"The type '{typeof (T).Name}' is not implemented, only string and byte[] are supported as data types."),
            };

            using var aes = System.Security.Cryptography.Aes.Create ();
            var iv = new byte[aes.BlockSize >> 3];

            System.Array.Copy (array, iv, iv.Length);

            aes.Key = System.Security.Cryptography.SHA256.HashData (System.Text.Encoding.UTF8.GetBytes (key));
            aes.IV = iv;

            using var ms = new System.IO.MemoryStream ();
            using (var cs = new System.Security.Cryptography.CryptoStream (ms, aes.CreateDecryptor (), System.Security.Cryptography.CryptoStreamMode.Write)) {
                cs.Write (array, iv.Length, array.Length - iv.Length);
                cs.FlushFinalBlock ();
            }

            return ms
                .ToArray ()
                .Convert<T> ();
        }

        public static void Encrypt<T> (in string key, in object data, out T encrypted)
            => encrypted = Encrypt<T> (in key, in data);
        public static void Encrypt<T> (ref T encrypted, in string key, in object data)
            => encrypted = Encrypt<T> (in key, in data);

        public static void Decrypt<T> (in string key, in object data, out T decrypted)
            => decrypted = Decrypt<T> (in key, in data);
        public static void Decrypt<T> (ref T decrypted, in string key, in object data)
            => decrypted = Decrypt<T> (in key, in data);
    }
}