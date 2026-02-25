using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNiveles : MonoBehaviour
{
    public void JugarNivel1()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void JugarNivel2()
    {
        // Bloqueado - no hace nada
    }

    public void JugarNivel3()
    {
        // Bloqueado - no hace nada
    }
}