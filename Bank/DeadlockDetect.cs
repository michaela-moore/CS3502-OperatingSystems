using System.Data;

public class DeadlockDetect {

    List <Thread> watchedThreads = [];

    
    public void WatchThread(Thread thread) {
        watchedThreads.Add(thread);
    }

    //function for checking if a deadlock has occured
    public void LogDeadlock(){

        Thread.Sleep(3000); //check every 3 seconds
        bool threadsAreBlocked = true;

        while (threadsAreBlocked){

            //Check the state of the thread
            foreach (Thread thread in watchedThreads){
                if (thread.ThreadState != ThreadState.WaitSleepJoin){ //thread is not blocked
                    threadsAreBlocked = false;
                    break;{
                }
            }

                if(threadsAreBlocked){
                    Console.WriteLine($"\nERROR : Deadlock detected in thread {thread.Name}");
                    return; // without this runs an infinite loop since threads are indefinitely blocked
                }

            }

        }
    }
}
