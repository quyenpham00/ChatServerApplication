// See https://aka.ms/new-console-template for more information
using ChatServerApplication.Uilities;

PasswordEncoder passwordEncoder = new PasswordEncoder();
Console.WriteLine(passwordEncoder.CheckPasswordInvalid("123"));
