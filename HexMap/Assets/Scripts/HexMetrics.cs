using UnityEngine;

/// <summary>
/// This static class controls the size of the Hex Cells 
/// outerRadius is equal to the size of one of the hex sides 
/// </summary>
public static class HexMetrics {

	public const float outerRadius = 10f;

	public const float innerRadius = outerRadius * 0.866025404f;

    /// <summary>
    /// These are the vertices of a Hex centered on 0f, 0f, 0f 
    /// on the XZ axis (for a 3d hex)
    /// </summary>
	public static Vector3[] corners = {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius) //the 7th vertex is actually the same as the first
            //I replicate the vector to make the formulas simpler, some of the "for" iterate up
            //to a 7th vertex when it should actually go back to the first; 
    };
}