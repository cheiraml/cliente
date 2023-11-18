using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InputController : MonoBehaviour
{
    public float tiempo=2;
    public float timer;
    public GamePlayer jugador;
    public event Action<Posiciondireccion> Disparo;
    public static InputController _Instance { get; set; }
    public event Action<Axis> onAxisChange;

    private static Axis axis = new Axis { Horizontal = 0, Vertical =0};
    Axis LastAxis = new Axis { Horizontal = 0, Vertical =0};

    void Start()
    {
        _Instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        var verticalInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");

        axis.Vertical = Mathf.RoundToInt(verticalInput);
        axis.Horizontal = Mathf.RoundToInt(horizontalInput);

        if (timer <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector2 pos = jugador.Punta.transform.position;
                Vector2 dir = jugador.Punta.transform.forward;
                Posiciondireccion posiciondireccion = new Posiciondireccion
                {
                    x = pos.x,
                    y = pos.y,
                    direccionx = dir.x,
                    direcciony = dir.y
                };
                Disparo?.Invoke(posiciondireccion);
                timer = tiempo;
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if (AxisChange())
        {
            LastAxis = new Axis { Horizontal = axis.Horizontal, Vertical = axis.Vertical };
            //NetworkController._Instance.Socket.Emit("move", axis);
            onAxisChange?.Invoke(axis);
        }
    }
 

    private bool AxisChange()
    {
        return (axis.Vertical != LastAxis.Vertical || axis.Horizontal !=LastAxis.Horizontal);
    }
}
public class Posiciondireccion
{
    public float x;
    public float y;
    public float direccionx;
    public float direcciony;
}

public class Axis
{
    public int Horizontal;
    public int Vertical;
}


