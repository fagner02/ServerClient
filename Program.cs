﻿using System.Net;
using System.Net.Sockets;
using SD;
class Program
{
    public static void Main(string[] args)
    {

        if (args.Length < 1)
        {
            // var inputStream = new PessoasInputStream();
            // inputStream.ReadFromFile();
            var outputStream = new PessoasOutputStream(new Pessoa[] {
                new () {Cpf = 100000002, Nome = "Joana", Idade = 19},
                new () {Cpf = 111000123, Nome = "Chico", Idade = 30},
                new () {Cpf = 111111111, Nome = "João", Idade = 25},
                new () {Cpf = 123123123, Nome = "Seu Zé", Idade = 45},
                new () {Cpf = 999999999, Nome = "Francisca", Idade = 50},
                });
            // outputStream.SaveToFile();
            // outputStream.Print();
            outputStream.SendToServer();
            return;
        }
        switch (args[0])
        {
            case "client":
                Client.Connect();
                break;
            case "server":
                Server.Setup();
                break;
        }
        return;
    }
}