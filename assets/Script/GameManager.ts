
import { _decorator, Component, Prefab, instantiate } from 'cc';
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

    @property({ type: Prefab })
    public applePrfb: Prefab | null = null;

    start() {
        this.locateApple();
    }

    locateApple() {
        let apple = instantiate(this.applePrfb);

        apple.setPosition(this.generateRandomPosition(), this.generateRandomPosition(), 0);
        apple.parent = this.node;
    }

    generateRandomPosition() {
        return Math.floor(Math.random() * GameManager._MAPSIZE * 2 + 1) - GameManager._MAPSIZE;
    }

    // update (deltaTime: number) {
    //     // [4]
    // }
}
