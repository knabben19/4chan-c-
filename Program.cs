using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Post
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}

class ThreadPost
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<Post> Replies { get; set; } = new List<Post>();
    public DateTime CreatedAt { get; set; }
}

class Program
{
    static List<ThreadPost> threads = new List<ThreadPost>();
    static string filePath = "threads.json";

    static void Main()
    {
        LoadData();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("==== TERMINALCHAN ====");
            Console.WriteLine("1 - Criar Tópico");
            Console.WriteLine("2 - Listar Tópicos");
            Console.WriteLine("3 - Responder Tópico");
            Console.WriteLine("4 - Deletar Tópico");
            Console.WriteLine("0 - Sair");
            Console.Write("\nEscolha: ");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    CreateThread();
                    break;
                case "2":
                    ListThreads();
                    break;
                case "3":
                    ReplyToThread();
                    break;
                case "4":
                    DeleteThread();
                    break;
                case "0":
                    SaveData();
                    return;
                default:
                    Console.WriteLine("Opção inválida.");
                    Pause();
                    break;
            }
        }
    }

    static void CreateThread()
    {
        Console.Write("\nTítulo do tópico: ");
        string title = Console.ReadLine();

        var thread = new ThreadPost
        {
            Id = threads.Count + 1,
            Title = title,
            CreatedAt = DateTime.Now
        };

        threads.Add(thread);
        SaveData();

        Console.WriteLine("Tópico criado com sucesso!");
        Pause();
    }

    static void ListThreads()
    {
        Console.WriteLine("\n==== TÓPICOS ====\n");

        foreach (var thread in threads)
        {
            Console.WriteLine($"[{thread.Id}] {thread.Title} ({thread.CreatedAt})");

            foreach (var reply in thread.Replies)
            {
                Console.WriteLine($"   ↳ {reply.Content} ({reply.CreatedAt})");
            }

            Console.WriteLine();
        }

        Pause();
    }

    static void ReplyToThread()
    {
        Console.Write("\nID do tópico: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var thread = threads.Find(t => t.Id == id);

            if (thread != null)
            {
                Console.Write("Mensagem (Anônimo): ");
                string content = Console.ReadLine();

                var reply = new Post
                {
                    Id = thread.Replies.Count + 1,
                    Content = content,
                    CreatedAt = DateTime.Now
                };

                thread.Replies.Add(reply);
                SaveData();

                Console.WriteLine("Resposta enviada!");
            }
            else
            {
                Console.WriteLine("Tópico não encontrado.");
            }
        }

        Pause();
    }

    static void DeleteThread()
    {
        Console.Write("\nID do tópico para deletar: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            threads.RemoveAll(t => t.Id == id);
            SaveData();
            Console.WriteLine("Tópico deletado.");
        }

        Pause();
    }

    static void SaveData()
    {
        var json = JsonSerializer.Serialize(threads, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    static void LoadData()
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            threads = JsonSerializer.Deserialize<List<ThreadPost>>(json) ?? new List<ThreadPost>();
        }
    }

    static void Pause()
    {
        Console.WriteLine("\nPressione qualquer tecla...");
        Console.ReadKey();
    }
}