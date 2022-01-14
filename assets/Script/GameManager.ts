import { _decorator, Component, Camera, Prefab, instantiate, Node, input, Input, Label, EventTouch, Vec3 } from 'cc';
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

enum GameState {
    GS_INIT,
    GS_PLAYING,
    GS_END,
};

@ccclass('GameManager')
export class GameManager extends Component {

    private static _MAPSIZE = 10;
    private static _NE_MAPSIZE = -10;

    private _apple: Node = null;

    private _curState: GameState = GameState.GS_INIT;

    @property({ type: Prefab })
    public applePrfb: Prefab | null = null;

    @property({ type: Prefab })
    public fencePrfb: Prefab | null = null;

    @property({ type: SnakeController })
    public snakeCtrl: SnakeController = null;

    @property({ type: Label })
    public scoreLabel: Label | null = null;

    @property({ type: Node })
    public startMenu: Node = null;

    // 3D camera reference
    @property(Camera)
    camera: Camera = null!;

    start() {
        this.curState = GameState.GS_INIT;
    }

    init() {
        this.initFence();
        this.initApple();

        if (this.startMenu) {
            this.startMenu.active = true;
        }

        if (this.scoreLabel) {
            this.scoreLabel.string = '0';
        }
    }

    private onTouchStart(event: EventTouch) {
        const camera = this.camera.camera;
        const pos = new Vec3();
        const location = event.getLocation();
        const screenPos = new Vec3(location.x, location.y, 0.03);
        camera.screenToWorld(pos, screenPos);

        this.snakeCtrl.moveTo(pos);
    }

    private initFence() {

        for (let i = GameManager._NE_MAPSIZE; i <= GameManager._MAPSIZE; i++) {
            for (let j = GameManager._NE_MAPSIZE; j <= GameManager._MAPSIZE; j++) {
                if (i == GameManager._NE_MAPSIZE || j == GameManager._NE_MAPSIZE
                    || i == GameManager._MAPSIZE || j == GameManager._MAPSIZE) {
                    let fence = instantiate(this.fencePrfb);
                    fence.setPosition(i, j, 0);
                    fence.parent = this.node;
                }
            }
        }
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
        let positionBound = GameManager._MAPSIZE - 1;
        return Math.floor(Math.random() * positionBound * 2 + 1) - positionBound;
    }

    update(deltaTime: number) {
        switch (this._curState) {
            case GameState.GS_PLAYING:
                if (this.snakeCtrl.canEatApple(this._apple.getPosition())) {
                    this.locateApple();
                    this.snakeCtrl.addTail();
                    this.onEatApple();
                }

                if (this.snakeCtrl.isOut(GameManager._MAPSIZE)) {
                    this.curState = GameState.GS_END;
                }
        }
    }

    private onEatApple() {
        this.scoreLabel.string = '' + this.snakeCtrl.getScore();
    }

    private set curState(value: GameState) {
        switch (value) {
            case GameState.GS_INIT:
                this.init();
                break;
            case GameState.GS_PLAYING:
                if (this.startMenu) {
                    this.startMenu.active = false;
                }

                if (this.scoreLabel) {
                    this.scoreLabel.string = '0';
                }

                setTimeout(() => {
                    if (this.snakeCtrl) {
                        this.snakeCtrl.init();
                    }
                }, 0.1);

                input.on(Input.EventType.TOUCH_START, this.onTouchStart, this);
                break;
            case GameState.GS_END:
                if (this.startMenu) {
                    this.startMenu.active = true;
                }
                this.snakeCtrl.reset();
                input.off(Input.EventType.TOUCH_START, this.onTouchStart, this);
                break;
        }
        this._curState = value;
    }

    onStartButtonClicked() {
        this.curState = GameState.GS_PLAYING;
    }
}
