using UnityEngine;
using System.Collections;
using System.IO;

public class WritingSavabale : Savable
{
    public bool horizontal;

    public override void Save(StreamWriter output)
    {
        if (horizontal)
        {
            Vector2 start = new Vector3(transform.position.x - transform.localScale.x / 2, transform.position.y - transform.localScale.y / 2, 0);
            Vector2 end = new Vector3(transform.position.x + transform.localScale.x / 2, transform.position.y + transform.localScale.y / 2, 0);
            base.Save(output);
            output.Write(' ');
            output.Write(start.x);
            output.Write(' ');
            output.Write(transform.position.y);
            output.Write(' ');
            output.Write(end.x);
            output.Write(' ');
            output.Write(transform.position.y);
        }
        else
        {
            Vector2 start = new Vector3(transform.position.x - transform.localScale.x / 2, transform.position.y - transform.localScale.y / 2, 0);
            Vector2 end = new Vector3(transform.position.x + transform.localScale.x / 2, transform.position.y + transform.localScale.y / 2, 0);
            base.Save(output);
            output.Write(' ');
            output.Write(transform.position.x);
            output.Write(' ');
            output.Write(start.y);
            output.Write(' ');
            output.Write(transform.position.x);
            output.Write(' ');
            output.Write(end.y);
        }
    }
}
