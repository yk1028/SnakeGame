
import { _decorator, Component, Prefab, instantiate, Node, input, Input, game } from 'cc';
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

 enum GameState{
    GS_PLAYING,
    GS_END,
};

@ccclass('GameManager')
export class GameManager extends Component {

    private static _MAPSIZE = 5;

    private _apple: Node = null;
    private _curState: GameState = GameState.GS_PLAYING;

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
        switch(this._curState) {
            case GameState.GS_PLAYING:
                if (this.snakeCtrl.canEatApple(this._apple.getPosition())) {
                    this.locateApple();
                    this.snakeCtrl.addTail();
                }

                if (this.snakeCtrl.isHitTail()) {
                    this._curState = GameState.GS_END;
                    game.end();
                }
                break;
            case GameState.GS_END:
                break;
        }
    }
}
