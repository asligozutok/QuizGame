using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject //daha fazla storage space sağlıyor. Datayı kodda tutmak yerine objenin
//içinde tutup gerektiğinde methodların içine sokabilirsin.
{
    [TextArea(2,6)]
    [SerializeField] string question = "Enter new question text here";

    [SerializeField] string[] answers = new string[4]; 
    [SerializeField] int correctAnswerIndex;
    
    //getter method: read-only access to private variable without having to make it public
    //but being able get the data stored inside
    public string GetQuestion() //string çünkü void type doesn't return anything 
    {
        return question;
    } //bunun sayesinde question variableını returnleyebiliyoruz ama içindekini değiştiremiyoruz başka scriptten(başka classta)
    
    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIndex; 
    }

    public string GetAnswer(int index) //anlamı index isimle bir int kabul ediyor
    {
        return answers[index]; 
    }


    
}
