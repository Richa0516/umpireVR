# 第三者アセットの確認記録

確認日：2026年10月6日。対象はローカルのUnity/ampireVR。公開用コピーを許可したファイルだけに限定し、授業用Git履歴は持ち込んでいません。

## 確認した配布条件

Fantasy Skybox FREEの同梱Readmeが示す[公式配布ページ](https://assetstore.unity.com/packages/2d/textures-materials/sky/fantasy-skybox-free-18353)では、Standard Unity Asset Store EULAが指定されています。

[Unity Asset Store EULA](https://unity.com/legal/as-terms) Appendix 1の2.2.1と2.2.1.1は、原創の製品へ組み込んだ配布とその制限を定めています。モデルやテクスチャの元データをGitHubでそのまま再配布する許諾とは扱えないため、この公開から除外しました。無料であることを再配布可の根拠にはしていません。

## 素材ごとの扱い

| ローカルの内容 | 確認結果 | 公開での扱い |
| --- | --- | --- |
| Fantasy Skybox FREE | 公式ページで標準Asset Store EULAを確認 | テクスチャ・マテリアル・デモを除外 |
| Baseball Essentials / BaseballPitch | 手元に原データの公開再配布を認める許諾が見つからない | 全体を除外 |
| BBB_8752trisのモデル・テクスチャ・prefab | 原モデルの配布元と再配布許諾を確認できない | 除外。授業履歴で追加されたゲーム用スクリプトのみ掲載 |
| Animation / Sota_AnimationのFBX・モーション・prefab | 生成・取得元と公開再配布許諾を確認できない | 除外。ゲームに接続するスクリプトのみ掲載 |
| Fonts / TextMesh Proのフォント・素材 | 一部にOFL文書があるが、全素材の出所と条件を網羅できない | 今回はフォント・素材一式を同梱しない |
| Oculus / Plugins / Samples / VRTemplateAssets | SDK・Unityサンプル等が混在 | 本体・サンプルの転載をせず、Package Managerから取得する |
| Object / Resources / Material(s) / Textureなど | 由来が未確定の素材が混在 | 除外 |
| RecordedFrames | 実行時に記録された画像 | 除外 |
| Assets/Scenesの3シーン | 授業用シーン構成。外部素材のGUID参照がある。埋め込み頂点・インデックス・地形データは検出されなかった | 設定として掲載。参照先の素材は同梱しない |

不明な素材の権利を確認済みとするものではありません。不明なものを除外することで、これらの素材の原データを公開リポジトリへアップロードしない構成にしています。

## 実際の公開範囲

- Assets/Script内のC#とmeta
- Assets/Scenes内のC#、3つのシーンとmeta
- Assets/BBB_8752tris/Script内のC#とmeta
- Assets/Sota_Animation/Prehab_and_script内のC#とmeta
- Assets/controllersetting.csとmeta
- 上記フォルダのmeta
- ProjectSettings、Packagesの依存関係記録
- READMEとこの確認記録

Library、Logs、Temp、obj、IDEの生成物、原モデル、画像、音声、モーション、SDKバイナリは含めません。素材を復元する場合は、取得元の利用条件を満たして自分で取得してください。
