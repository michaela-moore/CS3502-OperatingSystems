using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

namespace InterprocessCommunications {
    public class PipeServer {
        public static void Main(String[] args) {

            String file = "/home/parallels/Documents/CS5302-OperatingSystems/InterprocessCommunications/sample_files";

            //Initialize a new process to read files in folder
            using Process folderReadProcess = new();
            String output = "";

            try{
                folderReadProcess.StartInfo.FileName = "ls"; //command for reading contents of a folder
                folderReadProcess.StartInfo.Arguments = file; //folder to be read
                folderReadProcess.StartInfo.UseShellExecute = false; //created directly from the executable file, since we want to read the output
                folderReadProcess.StartInfo.RedirectStandardOutput = true; //capture the output of the process
                folderReadProcess.Start();

                //Read the output of the process
                Console.WriteLine("...Reading files from the folder...");
                output = folderReadProcess.StandardOutput.ReadToEnd();
                Console.WriteLine($"Files in folder: {output}");
                folderReadProcess.WaitForExit();


                //Create a new pipe server to send files to the pipe client
            

                using var pipeServer = new NamedPipeServerStream("filesPipe");
                Console.WriteLine("Waiting to connect to the pipe server...");
                pipeServer.WaitForConnection();

                using (var writer = new StreamWriter(pipeServer)){
                    //write files returned to the pipe server
                    writer.WriteLine(output);
                    writer.Flush();
                    writer.Close();
                    Console.WriteLine($"Files {output} sent to the pipe server.");
                }

            }
            catch (Exception e) {
                Console.WriteLine($"Error: {e.Message}");
            
            }
        }
    }
}
