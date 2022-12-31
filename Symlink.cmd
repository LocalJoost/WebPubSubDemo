cd /d %~dp0
mkdir WebPubSubAppSymLink
mklink /D "WebPubSubAppSymLink\Assets" "%~dp0\WebPubSubApp\Assets"
mklink /D "WebPubSubAppSymLink\ProjectSettings" "%~dp0\WebPubSubApp\ProjectSettings"
mklink /D "WebPubSubAppSymLink\Packages" "%~dp0\WebPubSubApp\Packages"
pause