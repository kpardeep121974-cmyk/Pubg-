using UnityEngine;
using UnityEngine.UI;

public class PlayerReviveInteractor : MonoBehaviour
{
    public float reviveDistance = 3f;
    public float reviveTimeRequired = 5f; // रिवाइव होने में 5 सेकंड लगेंगे
    private float currentReviveTimer = 0f;
    private bool isReviving = false;
    private HealthSystem targetPlayerHealth;

    void Update()
    {
        // स्क्रीन के केंद्र से रेकास्ट (Raycast) करके सामने वाले प्लेयर को देखें
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reviveDistance))
        {
            HealthSystem hp = hit.collider.GetComponent<HealthSystem>();
            
            // अगर सामने वाला प्लेयर नॉक है और आपकी ही टीम का है
            if (hp != null && hp.isKnockedOut && !hp.isDead)
            {
                // स्क्रीन पर "Press F to Revive" का टेक्स्ट दिखाएं
                // (आप अपने HUDManager में इसके लिए एक टेक्स्ट जोड़ सकते हैं)

                if (Input.GetKey(KeyCode.F))
                {
                    isReviving = true;
                    targetPlayerHealth = hp;
                    currentReviveTimer += Time.deltaTime;

                    Debug.Log($"रिवाइव हो रहा है: {Mathf.RoundToInt((currentReviveTimer / reviveTimeRequired) * 100)}%");

                    if (currentReviveTimer >= reviveTimeRequired)
                    {
                        targetPlayerHealth.RevivePlayer();
                        ResetReviveState();
                    }
                    return;
                }
            }
        }

        // अगर बटन छोड़ दिया या प्लेयर दूर चला गया, तो प्रोग्रेस रीसेट करें
        if (Input.GetKeyUp(KeyCode.F) || !isReviving)
        {
            ResetReviveState();
        }
    }

    void ResetReviveState()
    {
        isReviving = false;
        currentReviveTimer = 0f;
        targetPlayerHealth = null;
    }
}
