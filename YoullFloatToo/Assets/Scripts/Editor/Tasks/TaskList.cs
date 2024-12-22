using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TaskListUtility
{
    [CreateAssetMenu(fileName = "TaskList", menuName = "TaskList", order = 0)]
public class TaskList : ScriptableObject {

    [SerializeField] List<string> tasksInLine = new List<string>();
    [SerializeField] List<string> informationInLine = new List<string>();

    public List<string> GetTasks(){
        return tasksInLine;
    }

    public List<string> GetInformation(){
        return informationInLine;
    }

    public void AddTasks(List<string> savedTasks, List<string> info){
        //tasksInLine.Clear();
        tasksInLine = savedTasks;
        informationInLine = info;


    }

    public void AddTask (string savedTask, string info){
        tasksInLine.Clear();
        tasksInLine.Add(savedTask);
        informationInLine.Clear();
        informationInLine.Add(info);

    }

    


    }
    
}



