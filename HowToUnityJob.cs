// Author: Wyatt Murray
// Date: 4/5/2024
// Description:  This script provides a fairly comprehensive explanation and demonstration of Unity Jobs. this is a work in progress, so this code may change in the future to reflect 
// Version: v1.0
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

public class HowToUnityJob : MonoBehaviour
{
    #region Explanation

    //  1. What are Unity Jobs?

    //Unity Jobs are small code sections that run on your CPU's additional threads, leveraging multithreading for improved performance.
    //This is crucial for larger projects where single-threaded processing can bottleneck performance and lower frame rates (FPS).

    //  2. Why Use Unity Jobs?

    //They offer two main advantages:
    //Performance: Jobs allow performing complex calculations efficiently while the main thread continues working.This utilizes your entire CPU, not just a single core.
    //Responsiveness: The main thread remains available for handling user input and game logic, ensuring a smooth and responsive gameplay experience.

    //  3. Requirements:

    //Three Unity packages are recommended:
    //Unity Burst Compiler: Significantly improves job performance (optional).
    //Unity Job System: Enables job scheduling and execution.
    //Unity Mathematics: Provides high-performance vector math functions (optional).

    //jobs require 3 things
    //  1. The job itself
    //  2. A job handle/a way to schedule it
    //  3. a way of pulling the data out

    //sidenote: if you need to do matrix math AKA vector math, use Unity.Mathematics. its MUCH more performant (12~17% improvement),
    //but there are no built in mathmatical functions, so you will need to write your own.

    #endregion

    #region Demonstration and walkthrough
    //variables for showcase
    public int Test1 = 15;
    public int Test2 = 200;

    //a job is created by creating a struct that inherits from IJob or IJobParallelFor<T>
    //the burst compiler tag is new and makes jobified code MUCH faster (35~52% improvement)
    [BurstCompile]
    public struct customJob : IJob
    {
        //define variables like a normal struct
        //generally if they are not going to written to, you should use readonly
        public int Test1;
        public int Test2;
        //NativeArrays are uses to share the same place in memory for an array, and is a form of unsafe memory
        //generally, this should only be [WriteOnly] inside the struct unkless you plan to pass additional data in (genenrally not warranted)
        [WriteOnly] NativeArray<int> output;

        //make a constructor to put your variables in
        public customJob(int test1, int test2, NativeArray<int> outputStorage)
        {
            this.Test1 = test1;
            this.Test2 = test2;
            this.output = outputStorage;
        }

        //The execute method is where the actual code you want jobified to be ran.
        //you can define methods outside of it to do work
        public void Execute()
        {
            //in this example, we are doing some super simple math and then saving it to the native array
            int returnVal = testMethod(Test1, Test2);
            output[0] = returnVal;
        }

        int testMethod(int test1, int test2)
        {
            int returnVal = 0;
            for (int i = 0; i < test2; i++)
            {
                //some math problem you need done
                returnVal += i * test1 * test2;
            }
            return returnVal;
        }
    }

    private JobHandle ComputeCustomJob(int test1, int test2, NativeArray<int> array)
    {
        //create the struct
        customJob newJob = new customJob(test1, test2, array);

        //use job.Schedule to give a refernece to the job data and prepare the job to do work.
        //scheduling a job STARTS EXECUTING once it has all dependancies (for this example it should start immeadiately
        return newJob.Schedule();
    }

    private void Update()
    {
        //to use jobs, you must create and schdule them.
        //To do this, follow these steps

        //define the memory we want to use. we WILL need to dispose of it no matter the type of allocator when we are done with it
        //the variables for the native array are length, and the allocator type.
        //Allocator.Temp is for code that lasts 1 frame. they are the MOST performant.
        //Allocator.TempJob is for code that last under 4 frames (PRETTY HARD TO DO unless you are syncing up with fixed update)
        //Allocator.Persisant is the longest lasting ,which can be as long as the program is alive.
        NativeArray<int> array = new NativeArray<int>(1, Allocator.Temp);

        //create the job handle
        JobHandle newJob = ComputeCustomJob(Test1, Test2, array);

        //now, we wait for a completion state. we can use newJob.isCompleted as a bool to check if we want to do the math over multiple frames, but in order to sync up we need to catch the value on a frame.
        newJob.Complete();

        //now that we are synced up, we can retrieve the value(s)
        int saveVal = array[0];

        //now that we no longer need the array, we MUST dispose of it to prevent a memory leak
        array.Dispose();

        //debug it for demo
        Debug.Log(saveVal);
    }
    #endregion

}
