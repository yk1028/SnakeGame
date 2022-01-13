import { _decorator, Component, Vec3, input, Input, EventKeyboard, KeyCode, Prefab, instantiate, Node } from 'cc';
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

    private static _SNAKE_SPEED = 3;
    private static _INIT_NUM_OF_TAILS = 2;

    private _upDir: Vec3 = new Vec3(0, 1, 0);
    private _downDir: Vec3 = new Vec3(0, -1, 0);
    private _rightDir: Vec3 = new Vec3(1, 0, 0);
    private _leftDir: Vec3 = new Vec3(-1, 0, 0);
    private _nextDir: Vec3;

    private _snake: Node[] = [];
    private _snakePositions: Vec3[] = [];

    private _moveTime = 0;

    private _isActive = false;

    @property({ type: Prefab })
    public tailPrfb: Prefab | null = null;

    canEatApple(apple: Vec3) {
        return Vec3.distance(this.getHeadPosition(), apple) < 1;
    }

    addTail() {
        let tail = instantiate(this.tailPrfb);
        tail.setPosition(new Vec3(this._snakePositions[0]));
        tail.parent = this.node;

        this._snake.unshift(tail);
        this._snakePositions.unshift(new Vec3(tail.getPosition()));
    }

    isOut(mapSize: number) {
        let headPos = this.getHeadPosition();
        return headPos.x > mapSize - 1 || headPos.x < mapSize * -1 + 1
            || headPos.y > mapSize - 1 || headPos.y < mapSize * -1 + 1;
    }

    getScore() {
        return this._snake.length - 1 - SnakeController._INIT_NUM_OF_TAILS;
    }

    start() { }

    init() {
        this._isActive = true;
        this._nextDir = this._rightDir;

        this.initSnake();

        input.on(Input.EventType.KEY_DOWN, this.onKeyDown, this);
    }

    private initSnake() {
        this.initTails();
        this.initHead();
    }

    private initTails() {
        for (let i = SnakeController._INIT_NUM_OF_TAILS - 1; i >= 0; i--) {
            let tail = instantiate(this.tailPrfb);
            tail.setPosition(-1 * (i + 1), 0, 0);
            tail.parent = this.node;

            this._snake.push(tail);
            this._snakePositions.push(new Vec3(tail.getPosition()));
        }
    }

    private initHead() {
        let head = instantiate(this.tailPrfb);
        head.setPosition(0, 0, 0);
        head.parent = this.node;

        this._snake.push(head);
        this._snakePositions.push(new Vec3(head.getPosition()));
    }

    private onKeyDown(event: EventKeyboard) {
        switch (event.keyCode) {
            case KeyCode.ARROW_UP:
                if (this._nextDir != this._downDir) {
                    this._nextDir = this._upDir;
                }
                break;
            case KeyCode.ARROW_DOWN:
                if (this._nextDir != this._upDir) {
                    this._nextDir = this._downDir;
                }
                break;
            case KeyCode.ARROW_LEFT:
                if (this._nextDir != this._rightDir) {
                    this._nextDir = this._leftDir;
                }
                break;
            case KeyCode.ARROW_RIGHT:
                if (this._nextDir != this._leftDir) {
                    this._nextDir = this._rightDir;
                }
                break;
        }
    }

    update(deltaTime: number) {
        if (this._isActive) {

            deltaTime *= SnakeController._SNAKE_SPEED;

            this._moveTime += deltaTime;

            let headPos = this.updateHead(deltaTime);
            this.updateTail();

            if (this._moveTime >= 1) {
                this._moveTime = 0;

                this._snakePositions.shift();
                this._snakePositions.push(headPos);
            }
        }
    }

    private updateHead(deltaTime: number): Vec3 {
        let headPos = this.getHeadPosition();

        Vec3.add(headPos, headPos, new Vec3(this._nextDir.x * deltaTime, this._nextDir.y * deltaTime, 0));

        this._snake[this._snake.length - 1].setPosition(headPos);

        return headPos;
    }

    private updateTail() {
        for (let i = 0; i < this._snake.length - 1; i++) {
            let prevPos = this._snakePositions[i];
            let nextPos = this._snakePositions[i + 1];

            let curPos = new Vec3(prevPos.x + ((nextPos.x - prevPos.x) * this._moveTime), prevPos.y + ((nextPos.y - prevPos.y) * this._moveTime), 0);

            this._snake[i].setPosition(curPos);
        }
    }

    private getHeadPosition() {
        return new Vec3(this._snake[this._snakePositions.length - 1].getPosition());
    }

    reset() {
        for (let i = 0; i < this._snake.length; i++) {
            this._snake[i].removeFromParent();
        }
        this._snake = [];
        this._snakePositions = [];
        this._isActive = false;
        input.off(Input.EventType.KEY_DOWN, this.onKeyDown, this);
    }
}
