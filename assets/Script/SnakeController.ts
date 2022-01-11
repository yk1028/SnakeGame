import { _decorator, Component, Vec3, input, Input, EventKeyboard, Animation, KeyCode} from 'cc';
const { ccclass, property } = _decorator;

/**
 * Predefined variables
 * Name = SnakeController
 * DateTime = Tue Jan 11 2022 16:56:36 GMT+0900 (대한민국 표준시)
 * Author = yk1028
 * FileBasename = SnakeController.ts
 * FileBasenameNoExtension = SnakeController
 * URL = db://assets/Script/SnakeController.ts
 * ManualUrl = https://docs.cocos.com/creator/3.4/manual/en/
 *
 */

@ccclass('SnakeController')
export class SnakeController extends Component {

    private static _snakeSpeed = 0.01;

    private _curPos: Vec3 = new Vec3();

    private _upPos: Vec3 = new Vec3(0, SnakeController._snakeSpeed, 0);
    private _downPos: Vec3 = new Vec3(0, -1 * SnakeController._snakeSpeed, 0);
    private _rightPos: Vec3 = new Vec3(SnakeController._snakeSpeed, 0, 0);
    private _leftPos: Vec3 = new Vec3(-1 * SnakeController._snakeSpeed, 0, 0);
    private _nextPos: Vec3;

    // [2]
    // @property
    // serializableDummy = 0;

    start() {
        this._nextPos = this._rightPos;
        input.on(Input.EventType.KEY_DOWN, this.onKeyDown, this);
    }

    onKeyDown(event: EventKeyboard) {
        switch (event.keyCode) {
            case KeyCode.ARROW_UP:
                this._nextPos = this._upPos;
                break;
            case KeyCode.ARROW_DOWN:
                this._nextPos = this._downPos;
                break;
            case KeyCode.ARROW_LEFT:
                this._nextPos = this._leftPos;
                break;
            case KeyCode.ARROW_RIGHT:
                this._nextPos = this._rightPos;
                break;
        }
    }

    update(deltaTime: number) {
        this.node.getPosition(this._curPos);
        Vec3.add(this._curPos, this._curPos, this._nextPos);
        this.node.setPosition(this._curPos);
    }
}

/**
 * [1] Class member could be defined like this.
 * [2] Use `property` decorator if your want the member to be serializable.
 * [3] Your initialization goes here.
 * [4] Your update function goes here.
 *
 * Learn more about scripting: https://docs.cocos.com/creator/3.4/manual/en/scripting/
 * Learn more about CCClass: https://docs.cocos.com/creator/3.4/manual/en/scripting/ccclass.html
 * Learn more about life-cycle callbacks: https://docs.cocos.com/creator/3.4/manual/en/scripting/life-cycle-callbacks.html
 */
