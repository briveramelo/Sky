using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

public class PositionConverterTests
{
    [Test]
    public void WorldBoundariesVector2_ConvertToOthers()
    {
        TestWorldToOthers(Vector2.zero, ScreenSpace.ScreenSizePixels/2, Vector2.one * .5f);
        TestWorldToOthers(ScreenSpace.ScreenSizePixels, ScreenSpace.ScreenSizePixels, Vector2.one);
        TestWorldToOthers(-ScreenSpace.ScreenSizePixels, Vector2.zero, Vector2.zero);
        TestWorldToOthers(
            new Vector2(-ScreenSpace.ScreenSizePixels.x, ScreenSpace.ScreenSizePixels.y), 
            new Vector2( 0,ScreenSpace.ScreenSizePixels.y),
            new Vector2( 0,1));
        TestWorldToOthers(
            new Vector2(ScreenSpace.ScreenSizePixels.x, -ScreenSpace.ScreenSizePixels.y), 
            new Vector2(ScreenSpace.ScreenSizePixels.x, 0),
            new Vector2(1f, 0));
    }
    [Test]
    public void WorldBoundariesVector3_ConvertToOthers()
    {
        TestWorldToOthers(Vector3.zero, (Vector3)ScreenSpace.ScreenSizePixels/2, Vector3.one * .5f);
        TestWorldToOthers((Vector3)ScreenSpace.ScreenSizePixels, (Vector3)ScreenSpace.ScreenSizePixels, Vector3.one);
        TestWorldToOthers(-(Vector3)ScreenSpace.ScreenSizePixels, Vector3.zero, Vector3.zero);
        TestWorldToOthers(
            new Vector3(-ScreenSpace.ScreenSizePixels.x, ScreenSpace.ScreenSizePixels.y), 
            new Vector3( 0,ScreenSpace.ScreenSizePixels.y),
            new Vector3( 0,1));
        TestWorldToOthers(
            new Vector3(ScreenSpace.ScreenSizePixels.x, -ScreenSpace.ScreenSizePixels.y), 
            new Vector3(ScreenSpace.ScreenSizePixels.x, 0),
            new Vector3(1f, 0));
    }

    private void TestWorldToOthers(Vector2 worldPos, Vector2 expectedScreen, Vector2 expectedViewport)
    {
        var screenPos = worldPos.WorldPositionToPixels();
        var viewportPos = worldPos.WorldToViewportPosition();
        
        Assert.IsTrue(ApproximatelyEqual(screenPos, expectedScreen));
        Assert.IsTrue(ApproximatelyEqual(viewportPos, expectedViewport));
    }
    private void TestWorldToOthers(Vector3 worldPos, Vector3 expectedScreen, Vector3 expectedViewport)
    {
        var screenPos = worldPos.WorldPositionToPixels();
        var viewportPos = worldPos.WorldToViewportPosition();
        
        Assert.IsTrue(ApproximatelyEqual(screenPos, expectedScreen));
        Assert.IsTrue(ApproximatelyEqual(viewportPos, expectedViewport));
    }

    private bool ApproximatelyEqual(Vector2 a, Vector2 b, float tolerance = 0.1f)
    {
        return (a - b).magnitude < tolerance;
    }
}