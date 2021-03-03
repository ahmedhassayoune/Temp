using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{

    public static IEnumerator insertAnimation(GameObject[] obj_list, string obj_name_to_fade, Vector3 vector, float seconds, int target_alpha, GraphicalList list)
    {
        seconds /= 2f;

        for (float i = 0; i < seconds; i += Time.deltaTime)
        {
            foreach (GameObject obj in obj_list)
            {
                obj.transform.position += vector * Time.deltaTime / seconds;
            }
            yield return null;
        }

        list.drawObject();

        GameObject obj_to_fade = list.list_dict[obj_name_to_fade];
        MeshRenderer render = obj_to_fade.GetComponent<MeshRenderer>();

        render.material = Resources.Load<Material>("Materials/obj_transparent_mat");
        Color set_to_transparent = new Color(render.material.color.r, render.material.color.g, render.material.color.b, 0f);
        render.material.color = set_to_transparent;
        float stable_alpha_color = target_alpha - render.material.color.a;

        while (render.material.color.a < target_alpha)
        {
            Color new_color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, render.material.color.a + (stable_alpha_color * Time.deltaTime / seconds));
            render.material.color = new_color;
            yield return null;
        }

        render.material = Resources.Load<Material>("Materials/obj_ordinary_mat");
        list.drawObject();
    }

    public static IEnumerator removeAnimation(GameObject[] obj_list, string obj_name_to_fade, Vector3 vector, float seconds, int target_alpha, GraphicalList list)
    {
        seconds /= 2f;

        GameObject obj_to_fade = list.list_dict[obj_name_to_fade];
        MeshRenderer render = obj_to_fade.GetComponent<MeshRenderer>();

        list.list_dict.Remove(obj_name_to_fade);

        render.material = Resources.Load<Material>("Materials/obj_transparent_mat");
        Color set_to_transparent = new Color(render.material.color.r, render.material.color.g, render.material.color.b, 1f);
        render.material.color = set_to_transparent;
        float stable_alpha_color = render.material.color.a - target_alpha;

        while (render.material.color.a > target_alpha)
        {
            Color new_color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, render.material.color.a - (stable_alpha_color * Time.deltaTime / seconds));
            render.material.color = new_color;
            yield return null;
        }

        for (float i = 0; i < seconds; i += Time.deltaTime)
        {
            foreach (GameObject obj in obj_list)
            {
                obj.transform.position += vector * Time.deltaTime / seconds;
            }
            yield return null;
        }

        list.drawObject();
    }

    public static IEnumerator swap(GameObject obj1, GameObject obj2, float seconds, GraphicalList list)
    {
        Vector3 vector_obj1 = new Vector3(obj2.transform.position.x - obj1.transform.position.x, 0, 0);
        Vector3 vector_obj2 = new Vector3(obj1.transform.position.x - obj2.transform.position.x, 0, 0);

        for (float i = 0; i < seconds; i += Time.deltaTime)
        {
            obj1.transform.position += vector_obj1 * Time.deltaTime / seconds;
            obj2.transform.position += vector_obj2 * Time.deltaTime / seconds;

            yield return null;
        }

        list.drawObject();
    }

}
