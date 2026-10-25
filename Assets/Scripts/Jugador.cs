using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float fuerza = 20f;
    public float velocidadMax = 5f;
    public float fuerzaSalto = 7f;
    public float limiteCaida = -8f;
    public GameObject mensajeVictoria;

    private Rigidbody2D rb;
    private Vector3 posicionInicial;
    private bool enSuelo = false;
    private bool gano = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionInicial = transform.position;

        if (mensajeVictoria != null)
            mensajeVictoria.SetActive(false);
    }

    void Update()
    {
        if (gano) return;

        // Leemos las teclas una sola vez
        bool izquierda = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool derecha = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        // Movimiento lateral
        if (izquierda)
        {
            rb.AddForce(Vector2.left * fuerza);
        }
        if (derecha)
        {
            rb.AddForce(Vector2.right * fuerza);
        }

        // Si no presiona ninguna tecla lateral, se detiene en seco (solo en X)
        if (!izquierda && !derecha)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        // Limitar la velocidad horizontal para que no acelere sin fin
        float vx = Mathf.Clamp(rb.linearVelocity.x, -velocidadMax, velocidadMax);
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);

        // Salto: un solo impulso al presionar Espacio, solo si está en el suelo
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            enSuelo = false;
        }

        // Caída al vacío
        if (transform.position.y < limiteCaida)
        {
            Reiniciar();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Plataforma"))
        {
            enSuelo = true;
        }

        if (col.gameObject.CompareTag("Meta"))
        {
            gano = true;
            rb.linearVelocity = Vector2.zero;
            Debug.Log("¡Ganaste!");

            if (mensajeVictoria != null)
                mensajeVictoria.SetActive(true);
        }
    }

    void Reiniciar()
    {
        transform.position = posicionInicial;
        rb.linearVelocity = Vector2.zero;
        enSuelo = false;
    }
}       