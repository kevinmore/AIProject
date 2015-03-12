using UnityEngine;

public class StepsHandlerExample : MonoBehaviour
{
    private CharacterMotor charMot;
    private Vector3 displacement;
    private float iniBackSpeed;
    private float iniForSpeed;
    private float iniSideSpeed;
    private Vector3 lastPos;
    private float slowBackSpeed;
    private float slowForSpeed;
    private float slowSideSpeed;
    public float slowWalkVolume = 0.1f;
    private bool onetime;
    public float normalWalkRate = 0.7f;
    public float slowWalkRate = 1.5f;

    private void Start()
    {
        lastPos = transform.position;
        charMot = GetComponent<CharacterMotor>();
        iniForSpeed = charMot.movement.maxForwardSpeed;
        iniBackSpeed = charMot.movement.maxBackwardsSpeed;
        iniSideSpeed = charMot.movement.maxSidewaysSpeed;

        slowBackSpeed = charMot.movement.maxBackwardsSpeed - 6.0f;
        slowForSpeed = charMot.movement.maxForwardSpeed - 7.0f;
        slowSideSpeed = charMot.movement.maxSidewaysSpeed - 5.0f;

    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            audio.volume = slowWalkVolume;
            charMot.movement.maxForwardSpeed = slowForSpeed;
            charMot.movement.maxBackwardsSpeed = slowBackSpeed;
            charMot.movement.maxSidewaysSpeed = slowSideSpeed;
            if (onetime)
            {
                onetime = false;
                CancelInvoke("NormalWalk");
                InvokeRepeating("NormalWalk", 0f, slowWalkRate);
            }


        }
        else
        {
            audio.volume = 1f;
            charMot.movement.maxForwardSpeed = iniForSpeed;
            charMot.movement.maxBackwardsSpeed = iniBackSpeed;
            charMot.movement.maxSidewaysSpeed = iniSideSpeed;
            if (!onetime)
            {
                onetime = true;
                CancelInvoke("NormalWalk");
                InvokeRepeating("NormalWalk", 0f, normalWalkRate);
            }

          
        }
    }


    private void NormalWalk()
    {
        displacement = transform.position - lastPos;
        lastPos = transform.position;
        if (!charMot.IsJumping())
        {
            if (displacement.magnitude > 0.01)
            {
                if (!audio.isPlaying)
                {
                    audio.Play();
                }
            }
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(Screen.width/12, Screen.height - (Screen.height/4), Screen.width/1.1f, Screen.height/5),
                  "Hold Left Shift to walk slowly without noise! see the difference if you run behind the enemy!");
    }
}