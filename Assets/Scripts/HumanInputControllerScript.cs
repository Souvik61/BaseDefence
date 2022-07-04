using UnityEngine;

public class HumanInputControllerScript : MonoBehaviour
{
    public TankScript tankScript;

    int[] controlBits = new int[2];

    // Update is called once per frame
    void Update()
    {
        controlBits[0] = controlBits[1] = 0;
        if (Input.GetKey(KeyCode.W))
        {
            controlBits[0] = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            controlBits[0] = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            controlBits[1] = -1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            controlBits[1] = 1;
        }

        tankScript.Move(controlBits[0]);
        tankScript.Rotate(controlBits[1]);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            tankScript.MuzzleRotate(-1);

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            tankScript.MuzzleRotate(1);
        }

        if (Input.GetMouseButton(0))
        {
            tankScript.Shoot();
        }

    }
}
