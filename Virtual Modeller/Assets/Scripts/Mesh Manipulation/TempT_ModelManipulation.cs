public class TempT_ModelManipulation{
    void Main(string[] args){
        ModelManipulation test_mm = new ModelManipulation();
        TempT_PushDeformation(test_mm);
        TempT_PullDeformation(test_mm);
        TempT__sameGlobalPoint(test_mm);
    }

    public void TempT_PushDeformation(ModelManipulation mm){
        var x = mm.PushDeformation(
            new UnityEngine.Vector3(1,1,1),
            new UnityEngine.Vector3(2,2,2)
        );
    }

    public void TempT_PullDeformation(ModelManipulation mm){
        var x = mm.PullDeformation(
            new UnityEngine.Vector3(1,1,1),
            new UnityEngine.Vector3(2,2,2)
        );
    }

    public void TempT__sameGlobalPoint(ModelManipulation mm){
        /*var x = mm._sameGlobalPoint(
            new UnityEngine.Vector3(1.04,1.04,1.04),
            new UnityEngine.Vector3(1.05,1.05,1.05)
        );*/
    }
}