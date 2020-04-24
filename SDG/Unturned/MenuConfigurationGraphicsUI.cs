using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuConfigurationGraphicsUI
	{
		public MenuConfigurationGraphicsUI()
		{
			MenuConfigurationGraphicsUI.localization = Localization.read("/Menu/Configuration/MenuConfigurationGraphics.dat");
			MenuConfigurationGraphicsUI.container = new Sleek();
			MenuConfigurationGraphicsUI.container.positionOffset_X = 10;
			MenuConfigurationGraphicsUI.container.positionOffset_Y = 10;
			MenuConfigurationGraphicsUI.container.positionScale_Y = 1f;
			MenuConfigurationGraphicsUI.container.sizeOffset_X = -20;
			MenuConfigurationGraphicsUI.container.sizeOffset_Y = -20;
			MenuConfigurationGraphicsUI.container.sizeScale_X = 1f;
			MenuConfigurationGraphicsUI.container.sizeScale_Y = 1f;
			if (Provider.isConnected)
			{
				PlayerUI.container.add(MenuConfigurationGraphicsUI.container);
			}
			else
			{
				MenuUI.container.add(MenuConfigurationGraphicsUI.container);
			}
			MenuConfigurationGraphicsUI.active = false;
			MenuConfigurationGraphicsUI.graphicsBox = new SleekScrollBox();
			MenuConfigurationGraphicsUI.graphicsBox.positionOffset_X = -405;
			MenuConfigurationGraphicsUI.graphicsBox.positionOffset_Y = 100;
			MenuConfigurationGraphicsUI.graphicsBox.positionScale_X = 0.5f;
			MenuConfigurationGraphicsUI.graphicsBox.sizeOffset_X = 640;
			MenuConfigurationGraphicsUI.graphicsBox.sizeOffset_Y = -200;
			MenuConfigurationGraphicsUI.graphicsBox.sizeScale_Y = 1f;
			MenuConfigurationGraphicsUI.graphicsBox.area = new Rect(0f, 0f, 5f, 1510f);
			MenuConfigurationGraphicsUI.container.add(MenuConfigurationGraphicsUI.graphicsBox);
			MenuConfigurationGraphicsUI.landmarkButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.landmarkButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.landmarkButton.positionOffset_Y = 30;
			MenuConfigurationGraphicsUI.landmarkButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.landmarkButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.landmarkButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Landmark_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.landmarkButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Landmark_Button_Tooltip");
			SleekButtonState sleekButtonState = MenuConfigurationGraphicsUI.landmarkButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache0 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache0 = new SwappedState(MenuConfigurationGraphicsUI.onSwappedLandmarkState);
			}
			sleekButtonState.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache0;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.landmarkButton);
			MenuConfigurationGraphicsUI.bloomToggle = new SleekToggle();
			MenuConfigurationGraphicsUI.bloomToggle.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.bloomToggle.positionOffset_Y = 100;
			MenuConfigurationGraphicsUI.bloomToggle.sizeOffset_X = 40;
			MenuConfigurationGraphicsUI.bloomToggle.sizeOffset_Y = 40;
			MenuConfigurationGraphicsUI.bloomToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Bloom_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle = MenuConfigurationGraphicsUI.bloomToggle;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache1 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache1 = new Toggled(MenuConfigurationGraphicsUI.onToggledBloomToggle);
			}
			sleekToggle.onToggled = MenuConfigurationGraphicsUI.<>f__mg$cache1;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.bloomToggle);
			MenuConfigurationGraphicsUI.chromaticAberrationToggle = new SleekToggle();
			MenuConfigurationGraphicsUI.chromaticAberrationToggle.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.chromaticAberrationToggle.positionOffset_Y = 150;
			MenuConfigurationGraphicsUI.chromaticAberrationToggle.sizeOffset_X = 40;
			MenuConfigurationGraphicsUI.chromaticAberrationToggle.sizeOffset_Y = 40;
			MenuConfigurationGraphicsUI.chromaticAberrationToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Chromatic_Aberration_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle2 = MenuConfigurationGraphicsUI.chromaticAberrationToggle;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache2 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache2 = new Toggled(MenuConfigurationGraphicsUI.onToggledChromaticAberrationToggle);
			}
			sleekToggle2.onToggled = MenuConfigurationGraphicsUI.<>f__mg$cache2;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.chromaticAberrationToggle);
			MenuConfigurationGraphicsUI.filmGrainToggle = new SleekToggle();
			MenuConfigurationGraphicsUI.filmGrainToggle.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.filmGrainToggle.positionOffset_Y = 200;
			MenuConfigurationGraphicsUI.filmGrainToggle.sizeOffset_X = 40;
			MenuConfigurationGraphicsUI.filmGrainToggle.sizeOffset_Y = 40;
			MenuConfigurationGraphicsUI.filmGrainToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Film_Grain_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle3 = MenuConfigurationGraphicsUI.filmGrainToggle;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache3 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache3 = new Toggled(MenuConfigurationGraphicsUI.onToggledFilmGrainToggle);
			}
			sleekToggle3.onToggled = MenuConfigurationGraphicsUI.<>f__mg$cache3;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.filmGrainToggle);
			MenuConfigurationGraphicsUI.cloudsToggle = new SleekToggle();
			MenuConfigurationGraphicsUI.cloudsToggle.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.cloudsToggle.positionOffset_Y = 250;
			MenuConfigurationGraphicsUI.cloudsToggle.sizeOffset_X = 40;
			MenuConfigurationGraphicsUI.cloudsToggle.sizeOffset_Y = 40;
			MenuConfigurationGraphicsUI.cloudsToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Clouds_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle4 = MenuConfigurationGraphicsUI.cloudsToggle;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache4 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache4 = new Toggled(MenuConfigurationGraphicsUI.onToggledCloudsToggle);
			}
			sleekToggle4.onToggled = MenuConfigurationGraphicsUI.<>f__mg$cache4;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.cloudsToggle);
			MenuConfigurationGraphicsUI.blendToggle = new SleekToggle();
			MenuConfigurationGraphicsUI.blendToggle.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.blendToggle.positionOffset_Y = 300;
			MenuConfigurationGraphicsUI.blendToggle.sizeOffset_X = 40;
			MenuConfigurationGraphicsUI.blendToggle.sizeOffset_Y = 40;
			MenuConfigurationGraphicsUI.blendToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Blend_Toggle_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.blendToggle.state = GraphicsSettings.blend;
			SleekToggle sleekToggle5 = MenuConfigurationGraphicsUI.blendToggle;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache5 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache5 = new Toggled(MenuConfigurationGraphicsUI.onToggledBlendToggle);
			}
			sleekToggle5.onToggled = MenuConfigurationGraphicsUI.<>f__mg$cache5;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.blendToggle);
			MenuConfigurationGraphicsUI.fogToggle = new SleekToggle();
			MenuConfigurationGraphicsUI.fogToggle.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.fogToggle.positionOffset_Y = 350;
			MenuConfigurationGraphicsUI.fogToggle.sizeOffset_X = 40;
			MenuConfigurationGraphicsUI.fogToggle.sizeOffset_Y = 40;
			MenuConfigurationGraphicsUI.fogToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Fog_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle6 = MenuConfigurationGraphicsUI.fogToggle;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache6 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache6 = new Toggled(MenuConfigurationGraphicsUI.onToggledFogToggle);
			}
			sleekToggle6.onToggled = MenuConfigurationGraphicsUI.<>f__mg$cache6;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.fogToggle);
			MenuConfigurationGraphicsUI.grassDisplacementToggle = new SleekToggle();
			MenuConfigurationGraphicsUI.grassDisplacementToggle.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.grassDisplacementToggle.positionOffset_Y = 400;
			MenuConfigurationGraphicsUI.grassDisplacementToggle.sizeOffset_X = 40;
			MenuConfigurationGraphicsUI.grassDisplacementToggle.sizeOffset_Y = 40;
			MenuConfigurationGraphicsUI.grassDisplacementToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Grass_Displacement_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle7 = MenuConfigurationGraphicsUI.grassDisplacementToggle;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache7 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache7 = new Toggled(MenuConfigurationGraphicsUI.onToggledGrassDisplacementToggle);
			}
			sleekToggle7.onToggled = MenuConfigurationGraphicsUI.<>f__mg$cache7;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.grassDisplacementToggle);
			MenuConfigurationGraphicsUI.foliageFocusToggle = new SleekToggle();
			MenuConfigurationGraphicsUI.foliageFocusToggle.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.foliageFocusToggle.positionOffset_Y = 450;
			MenuConfigurationGraphicsUI.foliageFocusToggle.sizeOffset_X = 40;
			MenuConfigurationGraphicsUI.foliageFocusToggle.sizeOffset_Y = 40;
			MenuConfigurationGraphicsUI.foliageFocusToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Foliage_Focus_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle8 = MenuConfigurationGraphicsUI.foliageFocusToggle;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache8 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache8 = new Toggled(MenuConfigurationGraphicsUI.onToggledFoliageFocusToggle);
			}
			sleekToggle8.onToggled = MenuConfigurationGraphicsUI.<>f__mg$cache8;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.foliageFocusToggle);
			MenuConfigurationGraphicsUI.ragdollsToggle = new SleekToggle();
			MenuConfigurationGraphicsUI.ragdollsToggle.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.ragdollsToggle.positionOffset_Y = 500;
			MenuConfigurationGraphicsUI.ragdollsToggle.sizeOffset_X = 40;
			MenuConfigurationGraphicsUI.ragdollsToggle.sizeOffset_Y = 40;
			MenuConfigurationGraphicsUI.ragdollsToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Ragdolls_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle9 = MenuConfigurationGraphicsUI.ragdollsToggle;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache9 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache9 = new Toggled(MenuConfigurationGraphicsUI.onToggledRagdollsToggle);
			}
			sleekToggle9.onToggled = MenuConfigurationGraphicsUI.<>f__mg$cache9;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.ragdollsToggle);
			MenuConfigurationGraphicsUI.debrisToggle = new SleekToggle();
			MenuConfigurationGraphicsUI.debrisToggle.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.debrisToggle.positionOffset_Y = 550;
			MenuConfigurationGraphicsUI.debrisToggle.sizeOffset_X = 40;
			MenuConfigurationGraphicsUI.debrisToggle.sizeOffset_Y = 40;
			MenuConfigurationGraphicsUI.debrisToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Debris_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle10 = MenuConfigurationGraphicsUI.debrisToggle;
			if (MenuConfigurationGraphicsUI.<>f__mg$cacheA == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cacheA = new Toggled(MenuConfigurationGraphicsUI.onToggledDebrisToggle);
			}
			sleekToggle10.onToggled = MenuConfigurationGraphicsUI.<>f__mg$cacheA;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.debrisToggle);
			MenuConfigurationGraphicsUI.blastToggle = new SleekToggle();
			MenuConfigurationGraphicsUI.blastToggle.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.blastToggle.positionOffset_Y = 600;
			MenuConfigurationGraphicsUI.blastToggle.sizeOffset_X = 40;
			MenuConfigurationGraphicsUI.blastToggle.sizeOffset_Y = 40;
			MenuConfigurationGraphicsUI.blastToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Blast_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle11 = MenuConfigurationGraphicsUI.blastToggle;
			if (MenuConfigurationGraphicsUI.<>f__mg$cacheB == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cacheB = new Toggled(MenuConfigurationGraphicsUI.onToggledBlastToggle);
			}
			sleekToggle11.onToggled = MenuConfigurationGraphicsUI.<>f__mg$cacheB;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.blastToggle);
			MenuConfigurationGraphicsUI.puddleToggle = new SleekToggle();
			MenuConfigurationGraphicsUI.puddleToggle.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.puddleToggle.positionOffset_Y = 650;
			MenuConfigurationGraphicsUI.puddleToggle.sizeOffset_X = 40;
			MenuConfigurationGraphicsUI.puddleToggle.sizeOffset_Y = 40;
			MenuConfigurationGraphicsUI.puddleToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Puddle_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle12 = MenuConfigurationGraphicsUI.puddleToggle;
			if (MenuConfigurationGraphicsUI.<>f__mg$cacheC == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cacheC = new Toggled(MenuConfigurationGraphicsUI.onToggledPuddleToggle);
			}
			sleekToggle12.onToggled = MenuConfigurationGraphicsUI.<>f__mg$cacheC;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.puddleToggle);
			MenuConfigurationGraphicsUI.glitterToggle = new SleekToggle();
			MenuConfigurationGraphicsUI.glitterToggle.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.glitterToggle.positionOffset_Y = 700;
			MenuConfigurationGraphicsUI.glitterToggle.sizeOffset_X = 40;
			MenuConfigurationGraphicsUI.glitterToggle.sizeOffset_Y = 40;
			MenuConfigurationGraphicsUI.glitterToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Glitter_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle13 = MenuConfigurationGraphicsUI.glitterToggle;
			if (MenuConfigurationGraphicsUI.<>f__mg$cacheD == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cacheD = new Toggled(MenuConfigurationGraphicsUI.onToggledGlitterToggle);
			}
			sleekToggle13.onToggled = MenuConfigurationGraphicsUI.<>f__mg$cacheD;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.glitterToggle);
			MenuConfigurationGraphicsUI.triplanarToggle = new SleekToggle();
			MenuConfigurationGraphicsUI.triplanarToggle.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.triplanarToggle.positionOffset_Y = 750;
			MenuConfigurationGraphicsUI.triplanarToggle.sizeOffset_X = 40;
			MenuConfigurationGraphicsUI.triplanarToggle.sizeOffset_Y = 40;
			MenuConfigurationGraphicsUI.triplanarToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Triplanar_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle14 = MenuConfigurationGraphicsUI.triplanarToggle;
			if (MenuConfigurationGraphicsUI.<>f__mg$cacheE == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cacheE = new Toggled(MenuConfigurationGraphicsUI.onToggledTriplanarToggle);
			}
			sleekToggle14.onToggled = MenuConfigurationGraphicsUI.<>f__mg$cacheE;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.triplanarToggle);
			MenuConfigurationGraphicsUI.skyboxReflectionToggle = new SleekToggle();
			MenuConfigurationGraphicsUI.skyboxReflectionToggle.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.skyboxReflectionToggle.positionOffset_Y = 800;
			MenuConfigurationGraphicsUI.skyboxReflectionToggle.sizeOffset_X = 40;
			MenuConfigurationGraphicsUI.skyboxReflectionToggle.sizeOffset_Y = 40;
			MenuConfigurationGraphicsUI.skyboxReflectionToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Skybox_Reflection_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle15 = MenuConfigurationGraphicsUI.skyboxReflectionToggle;
			if (MenuConfigurationGraphicsUI.<>f__mg$cacheF == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cacheF = new Toggled(MenuConfigurationGraphicsUI.onToggledSkyboxReflectionToggle);
			}
			sleekToggle15.onToggled = MenuConfigurationGraphicsUI.<>f__mg$cacheF;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.skyboxReflectionToggle);
			MenuConfigurationGraphicsUI.distanceSlider = new SleekSlider();
			MenuConfigurationGraphicsUI.distanceSlider.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.distanceSlider.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.distanceSlider.sizeOffset_Y = 20;
			MenuConfigurationGraphicsUI.distanceSlider.orientation = ESleekOrientation.HORIZONTAL;
			MenuConfigurationGraphicsUI.distanceSlider.addLabel(MenuConfigurationGraphicsUI.localization.format("Distance_Slider_Label", new object[]
			{
				25 + (int)(GraphicsSettings.distance * 75f)
			}), ESleekSide.RIGHT);
			SleekSlider sleekSlider = MenuConfigurationGraphicsUI.distanceSlider;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache10 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache10 = new Dragged(MenuConfigurationGraphicsUI.onDraggedDistanceSlider);
			}
			sleekSlider.onDragged = MenuConfigurationGraphicsUI.<>f__mg$cache10;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.distanceSlider);
			MenuConfigurationGraphicsUI.landmarkSlider = new SleekSlider();
			MenuConfigurationGraphicsUI.landmarkSlider.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.landmarkSlider.positionOffset_Y = 70;
			MenuConfigurationGraphicsUI.landmarkSlider.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.landmarkSlider.sizeOffset_Y = 20;
			MenuConfigurationGraphicsUI.landmarkSlider.orientation = ESleekOrientation.HORIZONTAL;
			MenuConfigurationGraphicsUI.landmarkSlider.addLabel(MenuConfigurationGraphicsUI.localization.format("Landmark_Slider_Label", new object[]
			{
				25 + (int)(GraphicsSettings.distance * 75f)
			}), ESleekSide.RIGHT);
			SleekSlider sleekSlider2 = MenuConfigurationGraphicsUI.landmarkSlider;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache11 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache11 = new Dragged(MenuConfigurationGraphicsUI.onDraggedLandmarkSlider);
			}
			sleekSlider2.onDragged = MenuConfigurationGraphicsUI.<>f__mg$cache11;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.landmarkSlider);
			MenuConfigurationGraphicsUI.landmarkSlider.isVisible = false;
			MenuConfigurationGraphicsUI.antiAliasingButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("FXAA")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("TAA"))
			});
			MenuConfigurationGraphicsUI.antiAliasingButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.antiAliasingButton.positionOffset_Y = 840;
			MenuConfigurationGraphicsUI.antiAliasingButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.antiAliasingButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.antiAliasingButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Anti_Aliasing_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.antiAliasingButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Anti_Aliasing_Button_Tooltip");
			SleekButtonState sleekButtonState2 = MenuConfigurationGraphicsUI.antiAliasingButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache12 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache12 = new SwappedState(MenuConfigurationGraphicsUI.onSwappedAntiAliasingState);
			}
			sleekButtonState2.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache12;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.antiAliasingButton);
			MenuConfigurationGraphicsUI.anisotropicFilteringButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("AF_Disabled")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("AF_Per_Texture")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("AF_Forced_On"))
			});
			MenuConfigurationGraphicsUI.anisotropicFilteringButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.anisotropicFilteringButton.positionOffset_Y = 880;
			MenuConfigurationGraphicsUI.anisotropicFilteringButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.anisotropicFilteringButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.anisotropicFilteringButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Anisotropic_Filtering_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.anisotropicFilteringButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Anisotropic_Filtering_Button_Tooltip");
			SleekButtonState sleekButtonState3 = MenuConfigurationGraphicsUI.anisotropicFilteringButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache13 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache13 = new SwappedState(MenuConfigurationGraphicsUI.onSwappedAnisotropicFilteringState);
			}
			sleekButtonState3.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache13;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.anisotropicFilteringButton);
			MenuConfigurationGraphicsUI.effectButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.effectButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.effectButton.positionOffset_Y = 920;
			MenuConfigurationGraphicsUI.effectButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.effectButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.effectButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Effect_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.effectButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Effect_Button_Tooltip");
			SleekButtonState sleekButtonState4 = MenuConfigurationGraphicsUI.effectButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache14 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache14 = new SwappedState(MenuConfigurationGraphicsUI.onSwappedEffectState);
			}
			sleekButtonState4.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache14;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.effectButton);
			MenuConfigurationGraphicsUI.foliageButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.foliageButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.foliageButton.positionOffset_Y = 960;
			MenuConfigurationGraphicsUI.foliageButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.foliageButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.foliageButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Foliage_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.foliageButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Foliage_Button_Tooltip");
			SleekButtonState sleekButtonState5 = MenuConfigurationGraphicsUI.foliageButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache15 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache15 = new SwappedState(MenuConfigurationGraphicsUI.onSwappedFoliageState);
			}
			sleekButtonState5.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache15;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.foliageButton);
			if (!SystemInfo.supportsInstancing)
			{
				MenuConfigurationGraphicsUI.foliageButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Warning_GPU_Instancing"), Color.red, ESleekSide.LEFT);
			}
			MenuConfigurationGraphicsUI.sunShaftsButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.sunShaftsButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.sunShaftsButton.positionOffset_Y = 1000;
			MenuConfigurationGraphicsUI.sunShaftsButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.sunShaftsButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.sunShaftsButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Sun_Shafts_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.sunShaftsButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Sun_Shafts_Button_Tooltip");
			SleekButtonState sleekButtonState6 = MenuConfigurationGraphicsUI.sunShaftsButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache16 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache16 = new SwappedState(MenuConfigurationGraphicsUI.onSwappedSunShaftsState);
			}
			sleekButtonState6.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache16;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.sunShaftsButton);
			MenuConfigurationGraphicsUI.lightingButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.lightingButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.lightingButton.positionOffset_Y = 1040;
			MenuConfigurationGraphicsUI.lightingButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.lightingButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.lightingButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Lighting_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.lightingButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Lighting_Button_Tooltip");
			SleekButtonState sleekButtonState7 = MenuConfigurationGraphicsUI.lightingButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache17 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache17 = new SwappedState(MenuConfigurationGraphicsUI.onSwappedLightingState);
			}
			sleekButtonState7.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache17;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.lightingButton);
			MenuConfigurationGraphicsUI.ambientOcclusionButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.ambientOcclusionButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.ambientOcclusionButton.positionOffset_Y = 1080;
			MenuConfigurationGraphicsUI.ambientOcclusionButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.ambientOcclusionButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.ambientOcclusionButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Ambient_Occlusion_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.ambientOcclusionButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Ambient_Occlusion_Button_Tooltip");
			SleekButtonState sleekButtonState8 = MenuConfigurationGraphicsUI.ambientOcclusionButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache18 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache18 = new SwappedState(MenuConfigurationGraphicsUI.onSwappedAmbientOcclusionState);
			}
			sleekButtonState8.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache18;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.ambientOcclusionButton);
			MenuConfigurationGraphicsUI.reflectionButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.reflectionButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.reflectionButton.positionOffset_Y = 1120;
			MenuConfigurationGraphicsUI.reflectionButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.reflectionButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.reflectionButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Reflection_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.reflectionButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Reflection_Button_Tooltip");
			SleekButtonState sleekButtonState9 = MenuConfigurationGraphicsUI.reflectionButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache19 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache19 = new SwappedState(MenuConfigurationGraphicsUI.onSwappedReflectionState);
			}
			sleekButtonState9.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache19;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.reflectionButton);
			MenuConfigurationGraphicsUI.planarReflectionButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.planarReflectionButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.planarReflectionButton.positionOffset_Y = 1160;
			MenuConfigurationGraphicsUI.planarReflectionButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.planarReflectionButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.planarReflectionButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Planar_Reflection_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.planarReflectionButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Planar_Reflection_Button_Tooltip");
			SleekButtonState sleekButtonState10 = MenuConfigurationGraphicsUI.planarReflectionButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache1A == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache1A = new SwappedState(MenuConfigurationGraphicsUI.onSwappedPlanarReflectionState);
			}
			sleekButtonState10.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache1A;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.planarReflectionButton);
			MenuConfigurationGraphicsUI.waterButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.waterButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.waterButton.positionOffset_Y = 1200;
			MenuConfigurationGraphicsUI.waterButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.waterButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.waterButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Water_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.waterButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Water_Button_Tooltip");
			SleekButtonState sleekButtonState11 = MenuConfigurationGraphicsUI.waterButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache1B == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache1B = new SwappedState(MenuConfigurationGraphicsUI.onSwappedWaterState);
			}
			sleekButtonState11.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache1B;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.waterButton);
			MenuConfigurationGraphicsUI.scopeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.scopeButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.scopeButton.positionOffset_Y = 1240;
			MenuConfigurationGraphicsUI.scopeButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.scopeButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.scopeButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Scope_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.scopeButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Scope_Button_Tooltip");
			SleekButtonState sleekButtonState12 = MenuConfigurationGraphicsUI.scopeButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache1C == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache1C = new SwappedState(MenuConfigurationGraphicsUI.onSwappedScopeState);
			}
			sleekButtonState12.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache1C;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.scopeButton);
			MenuConfigurationGraphicsUI.outlineButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.outlineButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.outlineButton.positionOffset_Y = 1280;
			MenuConfigurationGraphicsUI.outlineButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.outlineButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.outlineButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Outline_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.outlineButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Outline_Button_Tooltip");
			SleekButtonState sleekButtonState13 = MenuConfigurationGraphicsUI.outlineButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache1D == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache1D = new SwappedState(MenuConfigurationGraphicsUI.onSwappedOutlineState);
			}
			sleekButtonState13.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache1D;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.outlineButton);
			MenuConfigurationGraphicsUI.boneButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.boneButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.boneButton.positionOffset_Y = 1320;
			MenuConfigurationGraphicsUI.boneButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.boneButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.boneButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Bone_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.boneButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Bone_Button_Tooltip");
			SleekButtonState sleekButtonState14 = MenuConfigurationGraphicsUI.boneButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache1E == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache1E = new SwappedState(MenuConfigurationGraphicsUI.onSwappedBoneState);
			}
			sleekButtonState14.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache1E;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.boneButton);
			MenuConfigurationGraphicsUI.terrainButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.terrainButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.terrainButton.positionOffset_Y = 1360;
			MenuConfigurationGraphicsUI.terrainButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.terrainButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.terrainButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Terrain_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.terrainButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Terrain_Button_Tooltip");
			SleekButtonState sleekButtonState15 = MenuConfigurationGraphicsUI.terrainButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache1F == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache1F = new SwappedState(MenuConfigurationGraphicsUI.onSwappedTerrainState);
			}
			sleekButtonState15.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache1F;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.terrainButton);
			MenuConfigurationGraphicsUI.windButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
			});
			MenuConfigurationGraphicsUI.windButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.windButton.positionOffset_Y = 1400;
			MenuConfigurationGraphicsUI.windButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.windButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.windButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Wind_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.windButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Wind_Button_Tooltip");
			SleekButtonState sleekButtonState16 = MenuConfigurationGraphicsUI.windButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache20 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache20 = new SwappedState(MenuConfigurationGraphicsUI.onSwappedWindState);
			}
			sleekButtonState16.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache20;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.windButton);
			MenuConfigurationGraphicsUI.treeModeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("TM_Legacy")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("TM_SpeedTree_Fade_None")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("TM_SpeedTree_Fade_SpeedTree"))
			});
			MenuConfigurationGraphicsUI.treeModeButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.treeModeButton.positionOffset_Y = 1440;
			MenuConfigurationGraphicsUI.treeModeButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.treeModeButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.treeModeButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Tree_Mode_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.treeModeButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Tree_Mode_Button_Tooltip");
			SleekButtonState sleekButtonState17 = MenuConfigurationGraphicsUI.treeModeButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache21 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache21 = new SwappedState(MenuConfigurationGraphicsUI.onSwappedTreeModeState);
			}
			sleekButtonState17.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache21;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.treeModeButton);
			MenuConfigurationGraphicsUI.renderButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Deferred")),
				new GUIContent(MenuConfigurationGraphicsUI.localization.format("Forward"))
			});
			MenuConfigurationGraphicsUI.renderButton.positionOffset_X = 205;
			MenuConfigurationGraphicsUI.renderButton.positionOffset_Y = 1480;
			MenuConfigurationGraphicsUI.renderButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.renderButton.sizeOffset_Y = 30;
			MenuConfigurationGraphicsUI.renderButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Render_Mode_Button_Label"), ESleekSide.RIGHT);
			MenuConfigurationGraphicsUI.renderButton.tooltip = MenuConfigurationGraphicsUI.localization.format("Render_Mode_Button_Tooltip");
			SleekButtonState sleekButtonState18 = MenuConfigurationGraphicsUI.renderButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache22 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache22 = new SwappedState(MenuConfigurationGraphicsUI.onSwappedRenderState);
			}
			sleekButtonState18.onSwappedState = MenuConfigurationGraphicsUI.<>f__mg$cache22;
			MenuConfigurationGraphicsUI.graphicsBox.add(MenuConfigurationGraphicsUI.renderButton);
			MenuConfigurationGraphicsUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuConfigurationGraphicsUI.backButton.positionOffset_Y = -50;
			MenuConfigurationGraphicsUI.backButton.positionScale_Y = 1f;
			MenuConfigurationGraphicsUI.backButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.backButton.sizeOffset_Y = 50;
			MenuConfigurationGraphicsUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuConfigurationGraphicsUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton = MenuConfigurationGraphicsUI.backButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache23 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache23 = new ClickedButton(MenuConfigurationGraphicsUI.onClickedBackButton);
			}
			sleekButton.onClickedButton = MenuConfigurationGraphicsUI.<>f__mg$cache23;
			MenuConfigurationGraphicsUI.backButton.fontSize = 14;
			MenuConfigurationGraphicsUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuConfigurationGraphicsUI.container.add(MenuConfigurationGraphicsUI.backButton);
			MenuConfigurationGraphicsUI.defaultButton = new SleekButton();
			MenuConfigurationGraphicsUI.defaultButton.positionOffset_X = -200;
			MenuConfigurationGraphicsUI.defaultButton.positionOffset_Y = -50;
			MenuConfigurationGraphicsUI.defaultButton.positionScale_X = 1f;
			MenuConfigurationGraphicsUI.defaultButton.positionScale_Y = 1f;
			MenuConfigurationGraphicsUI.defaultButton.sizeOffset_X = 200;
			MenuConfigurationGraphicsUI.defaultButton.sizeOffset_Y = 50;
			MenuConfigurationGraphicsUI.defaultButton.text = MenuPlayConfigUI.localization.format("Default");
			MenuConfigurationGraphicsUI.defaultButton.tooltip = MenuPlayConfigUI.localization.format("Default_Tooltip");
			SleekButton sleekButton2 = MenuConfigurationGraphicsUI.defaultButton;
			if (MenuConfigurationGraphicsUI.<>f__mg$cache24 == null)
			{
				MenuConfigurationGraphicsUI.<>f__mg$cache24 = new ClickedButton(MenuConfigurationGraphicsUI.onClickedDefaultButton);
			}
			sleekButton2.onClickedButton = MenuConfigurationGraphicsUI.<>f__mg$cache24;
			MenuConfigurationGraphicsUI.defaultButton.fontSize = 14;
			MenuConfigurationGraphicsUI.container.add(MenuConfigurationGraphicsUI.defaultButton);
			MenuConfigurationGraphicsUI.updateAll();
		}

		public static void open()
		{
			if (MenuConfigurationGraphicsUI.active)
			{
				return;
			}
			MenuConfigurationGraphicsUI.active = true;
			MenuConfigurationGraphicsUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuConfigurationGraphicsUI.active)
			{
				return;
			}
			MenuConfigurationGraphicsUI.active = false;
			MenuConfigurationGraphicsUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onToggledBloomToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.bloom = state;
			GraphicsSettings.apply();
		}

		private static void onToggledChromaticAberrationToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.chromaticAberration = state;
			GraphicsSettings.apply();
		}

		private static void onToggledFilmGrainToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.filmGrain = state;
			GraphicsSettings.apply();
		}

		private static void onToggledCloudsToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.clouds = state;
			GraphicsSettings.apply();
		}

		private static void onToggledBlendToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.blend = state;
			GraphicsSettings.apply();
		}

		private static void onToggledFogToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.fog = state;
			GraphicsSettings.apply();
		}

		private static void onToggledGrassDisplacementToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.grassDisplacement = state;
			GraphicsSettings.apply();
		}

		private static void onToggledFoliageFocusToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.foliageFocus = state;
			GraphicsSettings.apply();
		}

		private static void onSwappedLandmarkState(SleekButtonState button, int index)
		{
			GraphicsSettings.landmarkQuality = (EGraphicQuality)index;
			GraphicsSettings.apply();
			MenuConfigurationGraphicsUI.landmarkSlider.isVisible = (GraphicsSettings.landmarkQuality != EGraphicQuality.OFF);
		}

		private static void onToggledRagdollsToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.ragdolls = state;
			GraphicsSettings.apply();
		}

		private static void onToggledDebrisToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.debris = state;
			GraphicsSettings.apply();
		}

		private static void onToggledBlastToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.blast = state;
			GraphicsSettings.apply();
		}

		private static void onToggledPuddleToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.puddle = state;
			GraphicsSettings.apply();
		}

		private static void onToggledGlitterToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.glitter = state;
			GraphicsSettings.apply();
		}

		private static void onToggledTriplanarToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.triplanar = state;
			GraphicsSettings.apply();
		}

		private static void onToggledSkyboxReflectionToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.skyboxReflection = state;
			GraphicsSettings.apply();
		}

		private static void onDraggedDistanceSlider(SleekSlider slider, float state)
		{
			GraphicsSettings.distance = state;
			GraphicsSettings.apply();
			MenuConfigurationGraphicsUI.distanceSlider.updateLabel(MenuConfigurationGraphicsUI.localization.format("Distance_Slider_Label", new object[]
			{
				25 + (int)(state * 75f)
			}));
		}

		private static void onDraggedLandmarkSlider(SleekSlider slider, float state)
		{
			GraphicsSettings.landmarkDistance = state;
			GraphicsSettings.apply();
			MenuConfigurationGraphicsUI.landmarkSlider.updateLabel(MenuConfigurationGraphicsUI.localization.format("Landmark_Slider_Label", new object[]
			{
				(int)(state * 100f)
			}));
		}

		private static void onSwappedAntiAliasingState(SleekButtonState button, int index)
		{
			GraphicsSettings.antiAliasingType = (EAntiAliasingType)index;
			GraphicsSettings.apply();
		}

		private static void onSwappedAnisotropicFilteringState(SleekButtonState button, int index)
		{
			GraphicsSettings.anisotropicFilteringMode = (EAnisotropicFilteringMode)index;
			GraphicsSettings.apply();
		}

		private static void onSwappedEffectState(SleekButtonState button, int index)
		{
			GraphicsSettings.effectQuality = index + EGraphicQuality.LOW;
			GraphicsSettings.apply();
		}

		private static void onSwappedFoliageState(SleekButtonState button, int index)
		{
			GraphicsSettings.foliageQuality = (EGraphicQuality)index;
			GraphicsSettings.apply();
		}

		private static void onSwappedSunShaftsState(SleekButtonState button, int index)
		{
			GraphicsSettings.sunShaftsQuality = (EGraphicQuality)index;
			GraphicsSettings.apply();
		}

		private static void onSwappedLightingState(SleekButtonState button, int index)
		{
			GraphicsSettings.lightingQuality = (EGraphicQuality)index;
			GraphicsSettings.apply();
		}

		private static void onSwappedAmbientOcclusionState(SleekButtonState button, int index)
		{
			GraphicsSettings.ambientOcclusionQuality = (EGraphicQuality)index;
			GraphicsSettings.apply();
		}

		private static void onSwappedReflectionState(SleekButtonState button, int index)
		{
			GraphicsSettings.reflectionQuality = (EGraphicQuality)index;
			GraphicsSettings.apply();
		}

		private static void onSwappedPlanarReflectionState(SleekButtonState button, int index)
		{
			GraphicsSettings.planarReflectionQuality = index + EGraphicQuality.LOW;
			GraphicsSettings.apply();
		}

		private static void onSwappedWaterState(SleekButtonState button, int index)
		{
			GraphicsSettings.waterQuality = index + EGraphicQuality.LOW;
			GraphicsSettings.apply();
		}

		private static void onSwappedScopeState(SleekButtonState button, int index)
		{
			GraphicsSettings.scopeQuality = (EGraphicQuality)index;
			GraphicsSettings.apply();
		}

		private static void onSwappedOutlineState(SleekButtonState button, int index)
		{
			GraphicsSettings.outlineQuality = index + EGraphicQuality.LOW;
			GraphicsSettings.apply();
		}

		private static void onSwappedBoneState(SleekButtonState button, int index)
		{
			GraphicsSettings.boneQuality = index + EGraphicQuality.LOW;
			GraphicsSettings.apply();
		}

		private static void onSwappedTerrainState(SleekButtonState button, int index)
		{
			GraphicsSettings.terrainQuality = index + EGraphicQuality.LOW;
			GraphicsSettings.apply();
		}

		private static void onSwappedWindState(SleekButtonState button, int index)
		{
			GraphicsSettings.windQuality = (EGraphicQuality)index;
			GraphicsSettings.apply();
		}

		private static void onSwappedTreeModeState(SleekButtonState button, int index)
		{
			GraphicsSettings.treeMode = (ETreeGraphicMode)index;
		}

		private static void onSwappedRenderState(SleekButtonState button, int index)
		{
			GraphicsSettings.renderMode = (ERenderMode)index;
			GraphicsSettings.apply();
		}

		private static void onClickedBackButton(SleekButton button)
		{
			if (Player.player != null)
			{
				PlayerPauseUI.open();
			}
			else
			{
				MenuConfigurationUI.open();
			}
			MenuConfigurationGraphicsUI.close();
		}

		private static void onClickedDefaultButton(SleekButton button)
		{
			GraphicsSettings.restoreDefaults();
			MenuConfigurationGraphicsUI.updateAll();
		}

		private static void updateAll()
		{
			MenuConfigurationGraphicsUI.bloomToggle.state = GraphicsSettings.bloom;
			MenuConfigurationGraphicsUI.chromaticAberrationToggle.state = GraphicsSettings.chromaticAberration;
			MenuConfigurationGraphicsUI.filmGrainToggle.state = GraphicsSettings.filmGrain;
			MenuConfigurationGraphicsUI.cloudsToggle.state = GraphicsSettings.clouds;
			MenuConfigurationGraphicsUI.fogToggle.state = GraphicsSettings.fog;
			MenuConfigurationGraphicsUI.grassDisplacementToggle.state = GraphicsSettings.grassDisplacement;
			MenuConfigurationGraphicsUI.foliageFocusToggle.state = GraphicsSettings.foliageFocus;
			MenuConfigurationGraphicsUI.landmarkButton.state = (int)GraphicsSettings.landmarkQuality;
			MenuConfigurationGraphicsUI.ragdollsToggle.state = GraphicsSettings.ragdolls;
			MenuConfigurationGraphicsUI.debrisToggle.state = GraphicsSettings.debris;
			MenuConfigurationGraphicsUI.blastToggle.state = GraphicsSettings.blast;
			MenuConfigurationGraphicsUI.puddleToggle.state = GraphicsSettings.puddle;
			MenuConfigurationGraphicsUI.glitterToggle.state = GraphicsSettings.glitter;
			MenuConfigurationGraphicsUI.triplanarToggle.state = GraphicsSettings.triplanar;
			MenuConfigurationGraphicsUI.skyboxReflectionToggle.state = GraphicsSettings.skyboxReflection;
			MenuConfigurationGraphicsUI.distanceSlider.state = GraphicsSettings.distance;
			MenuConfigurationGraphicsUI.distanceSlider.updateLabel(MenuConfigurationGraphicsUI.localization.format("Distance_Slider_Label", new object[]
			{
				25 + (int)(GraphicsSettings.distance * 75f)
			}));
			MenuConfigurationGraphicsUI.landmarkSlider.state = GraphicsSettings.landmarkDistance;
			MenuConfigurationGraphicsUI.landmarkSlider.updateLabel(MenuConfigurationGraphicsUI.localization.format("Landmark_Slider_Label", new object[]
			{
				(int)(GraphicsSettings.landmarkDistance * 100f)
			}));
			MenuConfigurationGraphicsUI.landmarkSlider.isVisible = (GraphicsSettings.landmarkQuality != EGraphicQuality.OFF);
			MenuConfigurationGraphicsUI.antiAliasingButton.state = (int)GraphicsSettings.antiAliasingType;
			MenuConfigurationGraphicsUI.anisotropicFilteringButton.state = (int)GraphicsSettings.anisotropicFilteringMode;
			MenuConfigurationGraphicsUI.effectButton.state = GraphicsSettings.effectQuality - EGraphicQuality.LOW;
			MenuConfigurationGraphicsUI.foliageButton.state = (int)GraphicsSettings.foliageQuality;
			MenuConfigurationGraphicsUI.sunShaftsButton.state = (int)GraphicsSettings.sunShaftsQuality;
			MenuConfigurationGraphicsUI.lightingButton.state = (int)GraphicsSettings.lightingQuality;
			MenuConfigurationGraphicsUI.ambientOcclusionButton.state = (int)GraphicsSettings.ambientOcclusionQuality;
			MenuConfigurationGraphicsUI.reflectionButton.state = (int)GraphicsSettings.reflectionQuality;
			MenuConfigurationGraphicsUI.planarReflectionButton.state = GraphicsSettings.planarReflectionQuality - EGraphicQuality.LOW;
			MenuConfigurationGraphicsUI.waterButton.state = GraphicsSettings.waterQuality - EGraphicQuality.LOW;
			MenuConfigurationGraphicsUI.scopeButton.state = (int)GraphicsSettings.scopeQuality;
			MenuConfigurationGraphicsUI.outlineButton.state = GraphicsSettings.outlineQuality - EGraphicQuality.LOW;
			MenuConfigurationGraphicsUI.boneButton.state = GraphicsSettings.boneQuality - EGraphicQuality.LOW;
			MenuConfigurationGraphicsUI.terrainButton.state = GraphicsSettings.terrainQuality - EGraphicQuality.LOW;
			MenuConfigurationGraphicsUI.windButton.state = (int)GraphicsSettings.windQuality;
			MenuConfigurationGraphicsUI.treeModeButton.state = (int)GraphicsSettings.treeMode;
			MenuConfigurationGraphicsUI.renderButton.state = (int)GraphicsSettings.renderMode;
		}

		private static Local localization;

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon backButton;

		private static SleekButton defaultButton;

		private static SleekScrollBox graphicsBox;

		private static SleekToggle motionBlurToggle;

		private static SleekToggle bloomToggle;

		private static SleekToggle chromaticAberrationToggle;

		private static SleekToggle filmGrainToggle;

		private static SleekToggle cloudsToggle;

		private static SleekToggle blendToggle;

		private static SleekToggle fogToggle;

		private static SleekToggle grassDisplacementToggle;

		private static SleekToggle foliageFocusToggle;

		private static SleekToggle ragdollsToggle;

		private static SleekToggle debrisToggle;

		private static SleekToggle blastToggle;

		private static SleekToggle puddleToggle;

		private static SleekToggle glitterToggle;

		private static SleekToggle triplanarToggle;

		private static SleekToggle skyboxReflectionToggle;

		private static SleekSlider distanceSlider;

		private static SleekSlider landmarkSlider;

		private static SleekButtonState landmarkButton;

		public static SleekButtonState antiAliasingButton;

		public static SleekButtonState anisotropicFilteringButton;

		private static SleekButtonState effectButton;

		private static SleekButtonState foliageButton;

		private static SleekButtonState sunShaftsButton;

		private static SleekButtonState lightingButton;

		private static SleekButtonState ambientOcclusionButton;

		private static SleekButtonState reflectionButton;

		private static SleekButtonState planarReflectionButton;

		private static SleekButtonState waterButton;

		private static SleekButtonState scopeButton;

		private static SleekButtonState outlineButton;

		private static SleekButtonState boneButton;

		private static SleekButtonState terrainButton;

		private static SleekButtonState windButton;

		private static SleekButtonState treeModeButton;

		private static SleekButtonState renderButton;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache0;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache1;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache2;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache3;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache4;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache5;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache6;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache7;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache8;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache9;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheA;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheB;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheC;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheD;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheE;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheF;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache10;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache11;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache12;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache13;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache14;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache15;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache16;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache17;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache18;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache19;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache1A;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache1B;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache1C;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache1D;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache1E;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache1F;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache20;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache21;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache22;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache23;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache24;
	}
}
