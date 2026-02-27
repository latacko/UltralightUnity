#!/bin/bash

VERSION=$1
MESSAGE=${2:-"Release $VERSION"}

if [ -z "$VERSION" ]; then
    echo "Usage: ./release.sh v0.1.8 \"Message\""
    exit 1
fi

git tag -a "$VERSION" -m "$MESSAGE"
git push origin "$VERSION"

echo "Tagged and pushed $VERSION"