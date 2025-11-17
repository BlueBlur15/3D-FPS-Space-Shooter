using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Gun[] guns;      // assign in Inspector
    private int currentIndex = 0;

    // This flag prevents instant shooting after switching to auto
    private bool fireReleasedSinceSwitch = true;

    void Start()
    {
        if (guns != null && guns.Length > 0)
        {
            EquipGun(0);
        }
        else
        {
            Debug.LogWarning("WeaponManager: No gusn assigned!");
        }
    }

    void Update()
    {
        // Scroll wheel switching
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) NextGun();
        else if (scroll < 0f) PreviousGun();

        // Number keys (1/2/3)
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipGun(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipGun(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipGun(2);

        // Track if Fire1 (left mouse) has been released since switching
        if (!fireReleasedSinceSwitch)
        {
            if (!Input.GetButton("Fire1"))
            {
                // Once the user fully releases Fire1, we allow firing again
                fireReleasedSinceSwitch = true;
            }
        }

        // Tell active gun to handle input, but ONLY if we've seen a release
        Gun activeGun = GetActiveGun();
        if (activeGun != null && fireReleasedSinceSwitch)
        {
            activeGun.TryShoot();
        }
    }

    void EquipGun(int index)
    {
        if (guns == null || guns.Length == 0) return;
        if (index < 0 || index >= guns.Length) return;

        for (int i = 0; i < guns.Length; i++)
        {
            if (guns[i] != null)
                guns[i].gameObject.SetActive(i == index);
        }

        currentIndex = index;
        fireReleasedSinceSwitch = false; // block firing until Fire1 is released
        Debug.Log("Equipped: " + guns[currentIndex].gunName);
    }

    void NextGun()
    {
        if (guns == null || guns.Length == 0) return;
        int next = (currentIndex + 1) % guns.Length;
        EquipGun(next);
    }

    void PreviousGun()
    {
        if (guns == null || guns.Length == 0) return;
        int prev = (currentIndex - 1 + guns.Length) % guns.Length;
        EquipGun(prev);
    }

    Gun GetActiveGun()
    {
        if (guns == null || guns.Length == 0) return null;
        if (currentIndex < 0 || currentIndex >= guns.Length) return null;
        return guns[currentIndex];
    }
}
