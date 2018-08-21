using UnityEngine;

public class patch_ItemPotScript : ItemPotScript
{
    public Camera[] cams;
    public int camIndex;
    public Vector3 worldPos;

    public Font currentFont = null;
    
    public void OnGUI()
    {
        if (cams.Length == 0)
        {
            cams = FindObjectsOfType<Camera>();
            return;
        }

        Camera camera = null;
        foreach (var cam in cams)
        {
            if (cam.gameObject.name == "ExtCamera")
            {
                camera = cam;
            }
        }

        if (camera == null || exItemPrefab == null) 
            return;

        var centerY = Screen.height / 2;
        worldPos = camera.WorldToScreenPoint(transform.position);

        if (worldPos.y <= centerY)
        {
            var distToCenter = centerY - worldPos.y;
            worldPos.Set(worldPos.x, distToCenter + centerY, worldPos.z);
        }
        else
        {
            var distToCenter = worldPos.y - centerY;
            worldPos.Set(worldPos.x, centerY - distToCenter, worldPos.z);
        }

        AbstractItemBase component = exItemPrefab.GetComponent<AbstractItemBase>();

        if (component == null) 
            return;

        if (currentFont == null)
        {
            currentFont = Font.CreateDynamicFontFromOSFont("Consolas", 14);
        }
        
        var guistyle = new GUIStyle(GUI.skin.label);
        guistyle.fontStyle = FontStyle.Bold;
        guistyle.normal.textColor = Color.white;
        
        guistyle.font = currentFont;
        GUI.Label(new Rect(worldPos, new Vector3(100f, 100f)),
            $"{component.itemLabel ?? "unknown"} ({component.itemValue})",
            guistyle);
    }
}