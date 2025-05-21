using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelection : MonoBehaviour
{
    [SerializeField] private RectTransform selectionBox;
    private Vector2 startPos;
    private List<Unit> selectedUnits = new List<Unit>();

    private void Update()
    {
        HandleSelectionInput();
        HandleCommandInput();
    }

    private void HandleSelectionInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            selectionBox.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 currentPos = Input.mousePosition;
            UpdateSelectionBox(startPos, currentPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectionBox.gameObject.SetActive(false);

            // Si haces clic sin arrastrar, intenta atacar
            if (Vector2.Distance(Input.mousePosition, startPos) < 10f)
            {
                TryAttackAtCursor();
            }
            else
            {
                SelectUnitsInBounds();
            }
        }
    }

    private void HandleCommandInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                foreach (var unit in selectedUnits)
                {
                    unit.MoveTo(hit.point);
                }
            }
        }
    }

    private void TryAttackAtCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Atacar UNIDAD
            Unit targetUnit = hit.collider.GetComponent<Unit>();
            if (targetUnit != null)
            {
                foreach (var unit in selectedUnits)
                {
                    if (unit.team != targetUnit.team)
                    {
                        unit.MoveTo(targetUnit.transform.position);
                    }
                }
                return; // salimos si era unidad
            }

            // Atacar EDIFICIO
            Building targetBuilding = hit.collider.GetComponent<Building>();
            if (targetBuilding != null)
            {
                foreach (var unit in selectedUnits)
                {
                    // Comparar si el equipo es distinto
                    if ((int)unit.team != (int)targetBuilding.team)
                    {
                        unit.MoveTo(targetBuilding.transform.position);
                    }
                }
            }
        }
    }
    private void UpdateSelectionBox(Vector2 start, Vector2 end)
{
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        selectionBox.parent as RectTransform,
        start,
        null,
        out Vector2 localStart
    );

    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        selectionBox.parent as RectTransform,
        end,
        null,
        out Vector2 localEnd
    );

    Vector2 size = localEnd - localStart;

    selectionBox.anchoredPosition = localStart + size / 2f;
    selectionBox.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
}

    private void SelectUnitsInBounds()
    {
        selectedUnits.Clear();

        foreach (var unit in FindObjectsOfType<Unit>())
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
            if (screenPos.z < 0) continue;

            // Convertimos a coordenadas locales del canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                selectionBox.parent as RectTransform,
                screenPos,
                null,
                out Vector2 localPos
            );

            if (IsWithinSelectionBox(localPos) && unit.team == Team.Player)
            {
                unit.SetSelected(true);
                selectedUnits.Add(unit);
            }
            else
            {
                unit.SetSelected(false);
            }
        }
    }


    private bool IsWithinSelectionBox(Vector3 screenPos)
    {
        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);
        return screenPos.x >= min.x && screenPos.x <= max.x && screenPos.y >= min.y && screenPos.y <= max.y;
    }
}
