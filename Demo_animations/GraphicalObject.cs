using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Quasiment tout ce qui sera dessiné héritera de cette classe
Les seules exceptions sont:
 - les éléments du menu
 - les éléments de l'interface
 - 
*/
public abstract class GraphicalObject : MonoBehaviour
{
    // attributes
    public Vector3 dimensions;        // ex: {100, 40, 15} -> {lx, ly, lz}
    public Vector3 position;          // ex: {100, 100, -7} -> {x, y, z}

    //protected string name;          // ex: "List 1" // déjà intégré dans MonoBehaviour
    protected Parser.Variable variable;    // ex: Integer

    protected int representation;

    // getters
    public string getName() => name;
    public Parser.Variable getVariable() => variable;

    // methods
    public abstract void drawObject();       // dessine l'objet. doit être implémenté pour chaque sous-classe de GraphicalObject
}

// exemple: l'entier graphique
public class GraphicalInteger : GraphicalObject // on utilisera probablement une classe "nombre" qui fera int et flottant, voir classe abstraite
{
    public Parser.Integer val;
    public GraphicalInteger(string name, Parser.Integer val, Vector3 dimensions, Vector3 position, int representation) // representation : 0 -> dans une liste, 1 -> un index pointant sur une valeur de liste, 2 -> ...
    {
        this.name = name;
        this.val = val;
        this.dimensions = dimensions;
        this.position = position;
        this.representation = representation;
    }

    public override void drawObject()
    {

    }
}


public class GraphicalList : GraphicalObject
{
    public Parser.DynamicList list;
    public Dictionary<string, GameObject> list_dict;
    public int max_value;
    public GraphicalList(string name, Parser.DynamicList list, Vector3 dimensions, Vector3 position, int representation)
    {
        this.name = name;
        this.list = list;
        this.dimensions = dimensions;
        this.position = position;
        this.representation = representation;
    }

    public void insert(Parser.Integer index, Parser.Variable value)
    {
        list.insertValue(value, index);
        float time_animation = 2f;

        GameObject[] obj_list = new GameObject[list.length().getValue() - index.getValue() - 1];
        for (int i = index.getValue() + 1, u = 0; i < list.length().getValue(); i++, u++)
        {
            obj_list[u] = list_dict[list.atIndex(new Parser.Integer(i)).getName()];
        }

        StartCoroutine(Animation.insertAnimation(obj_list, value.getName(), new Vector3(1, 0, 0), time_animation, 1, this));
    }
    public void remove(Parser.Integer index)
    {
        string name = list.atIndex(index).getName();
        float time_animation = 3f;

        Destroy(list_dict[name], time_animation / 2f);
        list.removeValue(index);

        GameObject[] obj_list = new GameObject[list.length().getValue() - index.getValue()];
        for (int i = index.getValue(), u = 0; i < list.length().getValue(); i++, u++)
        {
            obj_list[u] = list_dict[list.atIndex(new Parser.Integer(i)).getName()];
        }

        StartCoroutine(Animation.removeAnimation(obj_list, name, new Vector3(-1, 0, 0), time_animation, 0, this));
    }

    public void swap(Parser.Integer index1, Parser.Integer index2)
    {
        Parser.Variable obj1_val = list.atIndex(index1);
        string obj1_name = list.atIndex(index1).getName();

        Parser.Variable obj2_val = list.atIndex(index2);
        string obj2_name = list.atIndex(index2).getName();

        list.removeValue(index1);
        list.insertValue(obj2_val, index1);
        list.atIndex(index1).setName(obj2_name);

        list.removeValue(index2);
        list.insertValue(obj1_val, index2);
        list.atIndex(index2).setName(obj1_name);

        StartCoroutine(Animation.swap(list_dict[obj1_name], list_dict[obj2_name], 2f, this));
    }

    public void listMaxValue(ref int max_value)
    {
        int length = list.length().getValue();
        max_value = list.atIndex(new Parser.Integer(0)).getValue();

        for(int i = 1; i < length; i++)
        {
            int val = list.atIndex(new Parser.Integer(i)).getValue();
            if (val > max_value)
                max_value = val;
        }
    }
    public override void drawObject()
    {
        int length = list.length().getValue();
        listMaxValue(ref max_value);

        for (int i = 0; i < length; i++)
        {
            string name = list.atIndex(new Parser.Integer(i)).getName();
            int obj_value = list.atIndex(new Parser.Integer(i)).getValue();
            if (list_dict.ContainsKey(name))
            {
                GameObject clone = list_dict[name];
                clone.transform.position = position + new Vector3(i, obj_value / 2f, 0);
                clone.transform.localScale = new Vector3(0.9f, obj_value, 1);
            }
            else
            {
                GameObject clone_prefab = Resources.Load<GameObject>("Prefabs/Quad_prefab");
                GameObject clone = Instantiate(clone_prefab, position + new Vector3(i, obj_value / 2f, 0), Quaternion.identity);
                //clone.transform.position = position + new Vector3(i, obj_value / 2f, 0);
                clone.transform.localScale = new Vector3(0.9f, obj_value, 1);

                GameObject floating_numb_prefab = Resources.Load<GameObject>("Prefabs/FloatingNumber");
                GameObject floating_number = Instantiate(floating_numb_prefab, new Vector3(clone.transform.position.x, position.y + 0.9f, clone.transform.position.z), Quaternion.identity);
                floating_number.GetComponent<TextMesh>().text = obj_value.ToString();
                floating_number.transform.SetParent(clone.transform);

                list_dict.Add(name, clone);
            }
        }
    }

}