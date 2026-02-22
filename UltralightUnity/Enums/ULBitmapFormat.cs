namespace UltralightUnity.Enums;

public enum ULBitmapFormat:byte
{
    /// <summary>
    /// Alpha channel only, 8-bits per pixel.
    ///
    /// Encoding: 8-bits per channel, unsigned normalized.
    ///
    /// Color-space: Linear (no gamma), alpha-coverage only.
    /// </summary>
    A8_UNORM,

    /// <summary>
    /// Blue Green Red Alpha channels, 32-bits per pixel.
    ///
    /// Encoding: 8-bits per channel, unsigned normalized.
    ///
    /// Color-space: sRGB gamma with premultiplied linear alpha channel.
    /// </summary>
    BGRA8_UNORM_SRGB
}