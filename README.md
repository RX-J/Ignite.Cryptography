# The Ignite.Cryptography class provides a simple way to symmetrically encrypt primitive data types.

## Important: The functions "Encrypt" and "Decrypt" both use the SHA256 algorithm to generate a key with 32 bytes from any password, if you store hashed passwords you should use a hashing algorithm other than SHA256 or use a salt! It should also be mentioned that the functionality has been kept compact and further security measures may need to be taken!

**The ```Encrypt``` function accepts any primitive data type and returns a ```byte[]``` or a ```string```, while the ```Decrypt``` function only accepts a ```byte[]``` or a ```string``` and returns any primitive data type. These primitive data types are:**
- ```byte```
- ```ushort```
- ```uint```
- ```ulong```
---
- ```sbyte```
- ```short```
- ```int```
- ```long```
---
- ```nint```
- ```nuint```
---
- ```float```
- ```double```
- ```decimal```
---
- ```bool```
- ```char```
- ```string```
---
**However, encryption and decryption only works reliably if the encrypted and decrypted data types are the same, e.g. a string decrypted as a string, or an int decrypted as an int.**

**To call the ```Encrypt``` function, you have these options:**

```cs
var encrypt = "Hello World!";

var encrypted = Cryptography.Encrypt<byte[]>("Password", encrypt); // Encrypts the string "Hello World!" using the password "Password" and returns a byte array
var encrypted = Cryptography.Encrypt<string>("Password", encrypt); // Encrypts the string "Hello World!" using the password "Password" and returns a string

Cryptography.Encrypt("Password", encrypt, out byte[] encrypted); // Encrypts the string "Hello World!" using the password "Password" and outputs the result to a byte array
Cryptography.Encrypt("Password", encrypt, out string encrypted); // Encrypts the string "Hello World!" using the password "Password" and outputs the result to a string

var encrypted = new byte[100];
Cryptography.Encrypt(ref encrypted, "Password", encrypt); // Encrypts the string "Hello World!" using the password "Password" and stores the result in a predefined byte array

var encrypted = "";
Cryptography.Encrypt(ref encrypted, "Password", encrypt); // Encrypts the string "Hello World!" using the password "Password" and stores the result in a predefined string
```

**To call the ```Decrypt``` function, you have these options:**

```cs
var decrypt = Cryptography.Encrypt<byte[]>("Password", "Hello World!");

var decrypted = Cryptography.Decrypt<TYPE>("Password", decrypt); // Decrypts the data using the password "Password" and returns the result as the specified type

Cryptography.Decrypt("Password", decrypt, out TYPE decrypted); // Decrypts the data using the password "Password" and outputs the result to a variable of the specified type

TYPE decrypted;
Cryptography.Decrypt(ref decrypted, "Password", decrypt); // Decrypts the data using the password "Password" and stores the result in a predefined variable of the specified type
```