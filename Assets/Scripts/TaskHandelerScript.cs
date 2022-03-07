using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TaskHandelerScript 
{
    private int result1 { get; set; }
    private int resultAsync { get; set; }
    private int getAsyncone { get; set; }
    public TaskHandelerScript() { }
    async public void CallTasks()
    {
        await CallAllAsyncTasks();
        CallAllTasks();
    }
    private void CallAllTasks()
    {
        var task1 = Task.Run( () =>
        {
            return TaskOne();
        } );
        var firstAwaiter = task1.GetAwaiter();
        firstAwaiter.OnCompleted( () =>
        {
            result1 = firstAwaiter.GetResult();
        } );
        var task2 = Task.Run( () =>
        {
            return TaskTwo( result1 );
        } );
        var secAwaiter = task2.GetAwaiter();
        firstAwaiter.OnCompleted( () =>
        {
            var result2 = secAwaiter.GetResult();
            TaskThree( result1 , result2 );
        } );
        // return a Task as a bool
        var taskBool1 = Task.Run( () =>
        {
            return TaskBoolOne();
        } );
        var firstBoolAwaiter = taskBool1.GetAwaiter();
        firstBoolAwaiter.OnCompleted( () =>
        {
            var resultBool1 = firstBoolAwaiter.GetResult();
            if (resultBool1)
            {
                TaskBoolTwo();
            }
        } );
    }
    private bool TaskBoolOne()
    {
        bool tsOne = true;
        Debug.Log( "tsOne " + tsOne );

        return tsOne;
    }
    private void TaskBoolTwo()
    {
        Debug.Log( "tsBoolTwo Called");
    }
    private int TaskOne()
    {
        int tsOne = 300;
        Debug.Log( "tsOne " + tsOne );

        return tsOne;
    }
    private int TaskTwo(int resultOne)
    {
        int tsTwo = resultOne + 200;
        Debug.Log( "tsTwo " + tsTwo );

        return tsTwo;
    }
    private int TaskThree(int resOne, int resTwo)
    {
        int tsThree = resOne + resTwo;
        Debug.Log( "tsThree " + tsThree );

        return tsThree;
    }
    //Async version
    async private Task CallAllAsyncTasks()
    {
        await CallTwoAsyncTaks();
        TaskAsyncThree(getAsyncone,resultAsync);
    }
    async private Task CallTwoAsyncTaks()
    {
        getAsyncone = await Task.Run( () =>
         {
             return TaskAsyncOne();
         } );
       resultAsync = TaskAsyncTwo( getAsyncone );
    }
   private int TaskAsyncOne()
    {
        int tsAsyncOne = 300;
        Debug.Log( "tsAsyncOne " + tsAsyncOne );

        return tsAsyncOne;
    }
    private int TaskAsyncTwo(int resultOne)
    {
        int tsAsyncTwo = resultOne + 200;
        Debug.Log( "tsAsyncTwo " + tsAsyncTwo );

        return tsAsyncTwo;
    }
    private int TaskAsyncThree(int resTwo, int resThree)
    {
        int total = resTwo + resThree;
        Debug.Log( "tsAsyncThree " + total);

        return total;
    }
}
