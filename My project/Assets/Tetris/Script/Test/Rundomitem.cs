#if UNITY_EDITOR
using NUnit.Framework;
using UnityEngine;
public class Rundomitem
{
    // A Test behaves as an ordinary method
    [Test]
    public void RundomItemTest()
    {
        TetorisLogic tetorisLogic = new TetorisLogic(() => { });
        var item = tetorisLogic.CreateTetriminoType();
        Debug.Log("itemname:" + item);
        for (int i = 0; i < 100; i++)
        {
            item = tetorisLogic.CreateTetriminoType();
            Debug.Log("itemname:" + item);
            if (item == TetorisLogic.TetriminoType.None)
            {
                Assert.Fail("Noneが出た");
            }
        }
    }

}
#endif