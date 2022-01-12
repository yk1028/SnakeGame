
import { _decorator, Component, Prefab, instantiate, Node } from 'cc';
import { SnakeController } from './SnakeController';
const { ccclass, property } = _decorator;

/**
 * Predefined variables
 * Name = GameManager
 * DateTime = Wed Jan 12 2022 13:48:49 GMT+0900 (대한민국 표준시)
 * Author = yk1028
 * FileBasename = GameManager.ts
 * FileBasenameNoExtension = GameManager
 * URL = db://assets/Script/GameManager.ts
 * ManualUrl = https://docs.cocos.com/creator/3.4/manual/en/
 *
 */

@ccclass('GameManager')
export class GameManager extends Component {

    private static _MAPSIZE = 5;

    private _apple: Node = null;

    @property({ type: Prefab })
    public applePrfb: Prefab | null = null;

    @property({ type: SnakeController })
    public snakeCtrl: SnakeController = null;

    start() {
        this.initApple();
    }

    private initApple() {
        this._apple = instantiate(this.applePrfb);
        this.locateApple();
        this._apple.parent = this.node;
    }

    private locateApple() {
        this._apple.setPosition(this.generateRandomPosition(), this.generateRandomPosition(), 0);
    }

    private generateRandomPosition() {
        return Math.floor(Math.random() * GameManager._MAPSIZE * 2 + 1) - GameManager._MAPSIZE;
    }

    update (deltaTime: number) {
        if (this.snakeCtrl.canEatApple(this._apple.getPosition())) {
            this.locateApple();
            this.snakeCtrl.addTail();
        }
    }
}
