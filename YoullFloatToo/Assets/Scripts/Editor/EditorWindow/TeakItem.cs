using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace TaskListUtility
{
public class TeakItem : VisualElement
{
    Toggle taskToggle;
    Label taskLabel;
    TextField taskTextField;

    public TeakItem(string taskText){
        VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TaskListEditor.path + "UIItem.uxml");
        this.Add(original.Instantiate());

        taskToggle = this.Q<Toggle>();
        taskLabel = this.Q<Label>();
        taskLabel.text = taskText;
        taskTextField = this.Q<TextField>();
        
        
        
    


    }

    

    public Toggle GetTaskToggle(){
        return taskToggle;
    }

    public Label GetLabel(){
        return taskLabel;
    }

    public TextField GetTaskTextField(){
        return taskTextField;
    }
}
}
