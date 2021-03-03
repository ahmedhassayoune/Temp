using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Test : MonoBehaviour
{

    public void Start()
    {

        GraphicalList graphList = gameObject.AddComponent<GraphicalList>();

        List<int> testList = new List<int>() { 5, 4, 3, 2, 1 };

        Parser.DynamicList list_test = Parser.VariableUtils.createDynamicList(testList);

        (list_test.atIndex(new Parser.Integer(0))).setName("int1");
        (list_test.atIndex(new Parser.Integer(1))).setName("int2");
        (list_test.atIndex(new Parser.Integer(2))).setName("int3");
        (list_test.atIndex(new Parser.Integer(3))).setName("int4");
        (list_test.atIndex(new Parser.Integer(4))).setName("int5");

        Dictionary<string, GameObject> list_dict_test = new Dictionary<string, GameObject>();

        graphList.list_dict = list_dict_test;
        graphList.list = list_test;

        graphList.drawObject(); // To update the dictionary

        /* INSERT DEMO
        Parser.Integer insert_numb = new Parser.Integer(5);
        insert_numb.setName("int6");

        graphList.insert(new Parser.Integer(2), insert_numb); // tester avec les index qui se trouvent a l'extremite
        */

        /* REMOVE DEMO
        graphList.remove(new Parser.Integer(1));
        */

        /* SWAP DEMO
        graphList.swap(new Parser.Integer(1), new Parser.Integer(4));
        */

        /*float t = Time.time;
        
        while (t < Time.time - 2)
        {
            
        }*/

        //Thread.Sleep(1000);

        //Debug.Log("Animation finished!");


        StartCoroutine(testTimeDelay());

        /* BUBBLE SORT DEMO
        StartCoroutine(bubbleSort());
        */

        IEnumerator testTimeDelay()
        {
            graphList.swap(new Parser.Integer(1), new Parser.Integer(4));
            yield return new WaitForSeconds(10f);
            graphList.remove(new Parser.Integer(1));
        }

        IEnumerator bubbleSort()
        {
            for (int j = 0; j <= list_test.length().getValue() - 2; j++)
            {
                for (int i = 0; i <= list_test.length().getValue() - 2; i++)
                {
                    if (list_test.atIndex(new Parser.Integer(i)).getValue() > list_test.atIndex(new Parser.Integer(i + 1)).getValue())
                    {
                        graphList.swap(new Parser.Integer(i), new Parser.Integer(i + 1));
                        yield return new WaitForSeconds(2.5f);
                    }
                }
            }
        }

        /*IEnumerator TimeDelayFunction()
        {

            for (int j = 0; j <= list_test.length().getValue() - 2; j++)
            {
                for (int i = 0; i <= list_test.length().getValue() - 2; i++)
                {
                    if (list_test.atIndex(new Parser.Integer(i)).getValue() > list_test.atIndex(new Parser.Integer(i + 1)).getValue())
                    {
                        graphList.swap(new Parser.Integer(i), new Parser.Integer(i + 1));
                        yield return new WaitForSeconds(1f);
                    }
                }
            }
            /*
            int smallest;

            for (int i = 0; i < list_test.length().getValue() - 1; i++)
            {
                smallest = i;
                for (int j = i + 1; j < list_test.length().getValue(); j++)
                {
                    if (list_test.atIndex(new Parser.Integer(j)).getValue() < list_test.atIndex(new Parser.Integer(smallest)).getValue())
                    {
                        smallest = j;
                    }
                }

                graphList.swap(new Parser.Integer(smallest), new Parser.Integer(i));
                yield return new WaitForSeconds(1f);
            }

            Debug.Log("List is sorted !");
        }
        */
    }

    /*
    public int testFunction(Parser.DynamicList list, Parser.Integer index1, Parser.Integer index2)
    {
        graph
        return 0;
    }
    */
}

