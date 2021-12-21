using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public int samples2D;
    public int samples3D;

    public double laserWidth;
    public double laserLength;

    private double[,] scan;
    private int layerMask = 1 << 8;

    // Stuff
    private bool start = false;


    // Start is called before the first frame update
    void Start()
    {
        resetScan();
        startScan();
    }


    public void resetScan()
    {
        scan = new double[samples3D, samples2D];
        Vector3 startPos = transform.position;
        startPos.z -= (float)laserLength / 2f;
        transform.position = startPos;
    }

    public void startScan()
    {
        // Moving scanner purely for visual purposes
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0.1f);
        start = true;

        // The actual 3D scan
        Vector3 startPos = transform.position;

        for (int index = 0; index < samples3D; index++)
        {

            startPos.z += (float)laserLength / (samples3D + 1);
            castRays(index, startPos);
        }
        Debug.Log(scan[50,0]);
    }

    public void stopScan()
    {
        start = false;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void castRays(int index3d, Vector3 startPos)
    {
        startPos.x -= (float)laserWidth / 2f;
        for (int index=0; index < samples2D; index++)
        {
            RaycastHit hit;

            startPos.x += (float)laserWidth / (samples2D + 1);

            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(startPos, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
            {
                //Debug.DrawRay(startPos, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
                //Debug.Log("Did Hit");
                scan[index3d, index] = transform.position.y - hit.distance;
            }
            else
            {
                //Debug.DrawRay(startPos, transform.TransformDirection(Vector3.down) * 1000, Color.white);
                //Debug.Log("Did not Hit");
            }
        }
    }
}
