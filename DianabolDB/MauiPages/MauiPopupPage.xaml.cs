using Microsoft.Maui.Controls;

namespace DianabolDB.MauiPages;

public partial class MauiPopupPage
{
	public MauiPopupPage()
	{
		InitializeComponent();
        CanBeDismissedByTappingOutsideOfPopup = true;
	}

    private void Scanner_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        scanner.IsDetecting = false;

        Close(e.Results[0].Value);
    }

    private void Popup_Tapped(object sender, EventArgs e)
    {
        // Close the popup when tapped
       Close();
    }
}