using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using Codice.Client.BaseCommands;

namespace TaskListUtility
{
public class TaskListEditor : EditorWindow
{
    VisualElement container;
    UnityEditor.UIElements.ObjectField savedTaskObjectField;
    Button loadTasksButton;
    TextField taskText;
    TextField taskInformation;
    Button addTaskButton;
    Button saveProgressButton;
    ScrollView taskListScrollView;
    TaskList taskListSO;
    ProgressBar taskProgressBar;
    ToolbarSearchField seachBox;
    Label notificationLabel;

    public const string path = "Assets/Scripts/Editor/EditorWindow/";

    [MenuItem("Window/Task List")]
    public static void ShowWindow(){
        TaskListEditor window = GetWindow<TaskListEditor>();
        window.titleContent = new GUIContent("Task List");
        
     
        window.minSize = new Vector2(500, 500);

    }

    public void CreateGUI()
    {
        container = rootVisualElement;
        VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path + "TeskListTemplate.uxml");
        container.Add(original.Instantiate());

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path + "TaskList.uss");
        container.styleSheets.Add(styleSheet);

        taskText = container.Q<TextField>("TaskText");
        taskText.RegisterCallback<KeyDownEvent>(AddTask);

        taskInformation = container.Q<TextField>("TaskInformation");
        taskInformation.RegisterCallback<KeyDownEvent>(AddTask);

        

        savedTaskObjectField = container.Q<UnityEditor.UIElements.ObjectField>("SaveTaskObjectField");
        savedTaskObjectField.objectType = typeof(TaskList);


        addTaskButton = container.Q<Button>("AddTask");
        addTaskButton.clicked += AddTask;

        saveProgressButton = container.Q<Button>("SaveProgressButton");
        saveProgressButton.clicked += SaveProgressMethod;

        loadTasksButton = container.Q<Button>("LoadTaskButton");
        loadTasksButton.clicked += LoadTask;

        seachBox = container.Q<ToolbarSearchField>("SearchBox");
        seachBox.RegisterValueChangedCallback(OnSearchTextChanged);

        taskProgressBar = container.Q<ProgressBar>("TaskProgressBar");

        notificationLabel = container.Q<Label>("NotificationLabel");
        UpdateModification("Please load a task list to Continue or create a new one");
         


        taskListScrollView = container.Q<ScrollView>("TaskLIst");
        //LogMenu();



    }

    void AddTask(){
        //Debug.Log("TaskAdded");

        if (!string.IsNullOrEmpty(taskText.value))
            {
                taskListScrollView.Add(CreateTask(taskText.value, taskInformation.value));
                SaveTask(taskText.value, taskInformation.value);
                taskText.value = "";
                taskText.Focus();
            }

        }

        private void SaveTask(string value, string info)
        {
            taskListSO.AddTask(value, info);
            EditorUtility.SetDirty(taskListSO);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        TeakItem CreateTask(string taskTest, string info)
        {
            TeakItem taskItem = new TeakItem(taskTest);
            taskItem.GetLabel().text = taskTest;
            taskItem.GetTaskToggle().RegisterValueChangedCallback(UpdateProgress);
            var taskTextField = taskItem.GetTaskTextField();
            taskItem.GetTaskTextField().SetValueWithoutNotify(info);

            return taskItem;
        }
       

        void AddTask(KeyDownEvent e){
        if (Event.current.Equals(Event.KeyboardEvent("Return"))){
            AddTask();
        }

        UpdateProgress();
        UpdateModification("Task Added");
    }

    void LoadTask()
{
    taskListSO = savedTaskObjectField.value as TaskList;

    if (taskListSO != null)
    {
        taskListScrollView.Clear();
        List<string> tasks = taskListSO.GetTasks();
        List<string> informations = taskListSO.GetInformation();

        // Asegúrate de que ambas listas tengan la misma longitud
        int minCount = Mathf.Min(tasks.Count, informations.Count);

        for (int i = 0; i < minCount; i++)
        {
            string task = tasks[i];
            string info = informations[i];
            taskListScrollView.Add(CreateTask(task, info));
            Debug.Log(task + " " + info);
        }
    }
    else
    {
        UpdateModification("Failed to load task list");
    }

    UpdateProgress();
    UpdateModification(taskListSO.name + " successfully loaded");
}
    void SaveProgressMethod(){
        if (taskListSO != null){
            List<string> tasks = new List<string>();
            List<string> info = new List<string>();

            foreach (TeakItem task in taskListScrollView.Children()){
                if (!task.GetTaskToggle().value){
                    tasks.Add(task.GetLabel().text);
                    info.Add(task.GetTaskTextField().text);
                }
            }

            taskListSO.AddTasks(tasks,info);
            EditorUtility.SetDirty(taskListSO);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            LoadTask();
            UpdateModification(taskListSO.name + " task save succesfully");
        }

        
    }

    void UpdateProgress(){
        int count = 0;
        int completed = 0;

        foreach(TeakItem task in taskListScrollView.Children()){

            if (task.GetTaskToggle().value){
                completed ++;


            }
            count ++;
        }

        if (count > 0){
            float progressCount = completed / (float)count;

            taskProgressBar.value = progressCount;

            taskProgressBar.title = string.Format("{0} %", Math.Round(progressCount * 1000) / 10f);
            UpdateModification("Progress Updated, dont forget to save");
        }
        else{
            taskProgressBar.value = 1;
            taskProgressBar.title = string.Format("{0} %", 100);
        }
    }

    void OnSearchTextChanged(ChangeEvent<string> e)
{
    string searchText = e.newValue.ToUpper();
    foreach (TeakItem task in taskListScrollView.Children())
    {
        string taskText = task.GetLabel().text.ToUpper();

        if (!string.IsNullOrEmpty(searchText) && taskText.Contains(searchText))
        {
            task.AddToClassList("highlight");
        }
        else{
            task.RemoveFromClassList("highlight");
        }
    }
}

void UpdateModification(string text){
    if(!string.IsNullOrEmpty(text)){
        notificationLabel.text = text;
    }
}

    private void LogMenu()
    {
        Debug.Log(taskText);
        Debug.Log(addTaskButton);
        Debug.Log(taskListScrollView);
        Debug.Log(loadTasksButton);
        Debug.Log(taskListScrollView);
        Debug.Log(savedTaskObjectField);
    }

    void UpdateProgress(ChangeEvent<bool> e){
        UpdateProgress();

    }
}
}
