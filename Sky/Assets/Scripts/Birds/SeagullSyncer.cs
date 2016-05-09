using UnityEngine;
using System.Collections.Generic;

public interface ISeagullSyncer {
    void RecordNewSeagull(ISeagull newSeagull);
    void ReportSeagullDown(ISeagull deadSeagull);
}

public class SeagullSyncer : MonoBehaviour, ISeagullSyncer {

    static int numSeagulls=0;
    List<ISeagull> seagulls = new List<ISeagull>();

    void ISeagullSyncer.RecordNewSeagull(ISeagull newSeagull) {
        seagulls.Add(newSeagull);
        newSeagull.UpdateSeagullNumber(numSeagulls);
        numSeagulls++;
    }

    void ISeagullSyncer.ReportSeagullDown(ISeagull deadSeagull) {
        seagulls.Remove(deadSeagull);
        numSeagulls--;
        int i = 0;
        seagulls.ForEach(seagull => { seagull.UpdateSeagullNumber(i); i++; });
    }
	
}
