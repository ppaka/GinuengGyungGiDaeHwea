using UnityEngine;

public class test : MonoBehaviour
{
    public float angle = 360, removeAngle = 90, rate = 30;
    public testobj prefab;
    public Transform tf;


    private void Start()
    {
        InvokeRepeating("Fire", 0, 3);
    }

    private void Fire()
    {
        var r = rate - 1;
        for (int i = 0; i < rate; i++)
        {
            var dir = tf.position - transform.position;
            var d = Instantiate(prefab, transform.position, Quaternion.identity);
            var z = angle / 2 * i / r - angle/4;
            print("i:" + i + ", z:" + z);
            var ang = Vector3.Angle(Vector3.down, dir.normalized);
            d.transform.Rotate(new Vector3(0, 0, z + ang / 2));
        }
    }
}
