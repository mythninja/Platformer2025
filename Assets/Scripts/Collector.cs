using UnityEngine;

public class Collector : MonoBehaviour
{
    //Activate when object with 2D collider hits it 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks the object they touched it a component that can be collected. If does save in variable other wise ignore 
        Iitem item = collision.GetComponent<Iitem>();
        if (item != null) 
        {
            //if item has the component call the collect 
            item.Collect(); 
        }
    }
}
