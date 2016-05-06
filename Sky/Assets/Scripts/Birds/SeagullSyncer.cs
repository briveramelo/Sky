using UnityEngine;
using System.Collections.Generic;

public interface ISeagullSyncer {
    int RecordNewSeagull(ISeagull newSeagull);
    void ReportSeagullDown(ISeagull deadSeagull);
}

public class SeagullSyncer : MonoBehaviour, ISeagullSyncer {

    static int numSeagulls=0;
    List<ISeagull> seagulls = new List<ISeagull>();

    int ISeagullSyncer.RecordNewSeagull(ISeagull newSeagull) {
        seagulls.Add(newSeagull);
        return ++numSeagulls;
    }

    void ISeagullSyncer.ReportSeagullDown(ISeagull deadSeagull) {
        seagulls.Remove(deadSeagull);
        numSeagulls--;
        int i = 0;
        seagulls.ForEach(seagull => { seagull.UpdateSeagullNumber(i, seagulls.Count); i++; });
    }
	
}
