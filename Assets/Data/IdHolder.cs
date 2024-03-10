using UnityEngine;

public class IdHolder : MonoBehaviour
{
    public string expiryDate;
    public string dateOfBirth;
    public Sprite image; // This will be the NPC's real picture or a fake one

   

    private void Awake()
    {
        
        float rand = Random.value;
        if (rand <= 0.80)
        {
            realID();
        }
        else
        {
            fakeId();
        }
    }

    void realID()
    {
        // Assign real values
        expiryDate = GenerateRealExpiryDate();
        dateOfBirth = GenerateRealDateOfBirth();

        Debug.Log($"Real ID Created - Expiry Date: {expiryDate}, Date of Birth: {dateOfBirth}, Image will be assigned after capture");

        TakePictureOfNPC();
    }

    void fakeId()
    {
        // Randomly assign fake or real values
        expiryDate = Random.value > 0.5f ? GenerateFakeExpiryDate() : GenerateRealExpiryDate();
        dateOfBirth = Random.value > 0.5f ? GenerateFakeDateOfBirth() : GenerateRealDateOfBirth();

        Debug.Log($"Fake ID Created - Expiry Date: {expiryDate} (Fake: {expiryDate}), Date of Birth: {dateOfBirth} (Fake: {dateOfBirth}), Image: Pre-made fake image assigned");
        AssignFakeImage();
    }


    void TakePictureOfNPC()
    {
        Debug.Log("TakePictureOfNPC called.");
        StartCoroutine(IDCAM.Instance.CaptureScreenshot(transform, (capturedSprite) =>
        {
            // Now you have a sprite ready to be assigned to the canvas viewer
            AssignSpriteToCanvas(capturedSprite);
        }));
    }

    void AssignFakeImage()
    {
        if (IDCAM.Instance.fakeImages.Count > 0)
        {
            int index = Random.Range(0, IDCAM.Instance.fakeImages.Count);
            image = IDCAM.Instance.fakeImages[index];
        }
    }
    string GenerateRealExpiryDate()
    {
        System.DateTime realExpiry = System.DateTime.Now.AddYears(Random.Range(1, 5));
        return realExpiry.ToString("MM/dd/yyyy");
    }

    
    string GenerateRealDateOfBirth()
    {
        System.DateTime dob = System.DateTime.Now.AddYears(-18).AddYears(-Random.Range(0, 22));
        return dob.ToString("MM/dd/yyyy");
    }

    
    string GenerateFakeExpiryDate()
    {
        System.DateTime fakeExpiry = System.DateTime.Now.AddYears(-Random.Range(1, 5));
        return fakeExpiry.ToString("MM/dd/yyyy");
    }

    
    string GenerateFakeDateOfBirth()
    {
        System.DateTime dob = System.DateTime.Now.AddYears(-Random.Range(10, 17));
        return dob.ToString("MM/dd/yyyy");
    }

    void AssignSpriteToCanvas(Sprite capturedSprite)
    {
        Debug.Log($"Received sprite: {capturedSprite}");

        if (capturedSprite != null)
        {
            image = capturedSprite; 
            if (IDDisplayManager.Instance != null)
            {

                IDDisplayManager.Instance.SetIDInfo(image, expiryDate, dateOfBirth);
            }
            else
            {
                Debug.LogError("IDDisplayManager  not found.");
            }
        }
        else
        {
            Debug.LogError("Captured sprite is null.");
        }
    }
}
