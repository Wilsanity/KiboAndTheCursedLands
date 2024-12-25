using System.Collections;
using System.Collections.Generic;
public interface IState {
    void onEnter(); // start 
    void Update();  //could also call this in unities update, same below
    void FixedUpdate();
    void onExit();
}
