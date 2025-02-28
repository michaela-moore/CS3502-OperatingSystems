using System.IO.Pipes;
namespace InterprocessCommunications;

public class PipeClient {
    public static void Main(){
        //Wait for the pipe server to start
        Thread.Sleep(3000);

        try {
            Console.WriteLine("Connecting to the pipe server...");

            //Create a new pipe client to read files from the filesPipe server
            using var pipeClient = new NamedPipeClientStream(".", "filesPipe", PipeDirection.In);

            
            pipeClient.Connect();
            Console.WriteLine("Connected to the pipe server.");
            using var reader = new StreamReader(pipeClient);


            //read files from pipeserver and display
            string readFilesFromServer = reader.ReadToEnd();
            Console.WriteLine($"Files read from server : {readFilesFromServer}");
        } 
        catch (Exception e) {
            Console.WriteLine($"Error: {e.Message}");
        }

    }
}