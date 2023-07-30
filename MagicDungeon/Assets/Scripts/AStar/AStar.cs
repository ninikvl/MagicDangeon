using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    /// <summary>
    /// Строит путь для комнаты от начальной позиции сетки до конечной позиции сетки и добавляет
    /// шаги перемещения в возвращаемый стек. Возвращает значение null, если путь не найден.
    /// </summary>
    public static Stack<Vector3> BuildPath(Room room, Vector3Int startGridPosition, Vector3Int endGridPosition)
    {
        // Корректировка позиции по нижним границам
        startGridPosition -= (Vector3Int)room.templateLowerBounds;
        endGridPosition -= (Vector3Int)room.templateLowerBounds;

        // Создание открытытого списока и закрытый хэш-набор
        List<Node> openNodeList = new List<Node>();
        HashSet<Node> closedNodeHashSet = new HashSet<Node>();

        // создание узлов сетки для поиска пути
        GridNodes gridNodes = new GridNodes(room.templateUpperBounds.x - room.templateLowerBounds.x + 1, 
            room.templateUpperBounds.y - room.templateLowerBounds.y + 1);

        Node startNode = gridNodes.GetGridNode(startGridPosition.x, startGridPosition.y);
        Node targetNode = gridNodes.GetGridNode(endGridPosition.x, endGridPosition.y);

        Node endPathNode = FindShortestPath(startNode, targetNode, gridNodes, openNodeList, closedNodeHashSet, room.instantiatedRoom);

        if (endPathNode != null)
        {
            return CreatePathStack(endPathNode, room);
        }
        return null;
    }

    /// <summary>
    /// получить кратчайший путь - возвращает конечный узел, если путь был найден, 
    /// в противном случае возвращает значение null.
    /// </summary>
    private static Node FindShortestPath(Node startNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, InstantiatedRoom instantiatedRoom)
    {
        // добавление начального узла в список открытых
        openNodeList.Add(startNode);

        // Перебор списока открытых узлов до тех пор, пока он не опустеет
        while (openNodeList.Count > 0)
        {
            // Сортировка списка
            openNodeList.Sort();

            // текущий узел = узел в открытом списке с наименьшей стоимостью
            Node currentNode = openNodeList[0];
            openNodeList.RemoveAt(0);

            // если текущий узел = целевой узел, то завершение
            if (currentNode == targetNode)
            {
                return currentNode;
            }

            // добавление текущего узла в закрытый список
            closedNodeHashSet.Add(currentNode);

            // Вычисление стоимости для каждого соседа текущего узла
            EvaluateCurrentNodeNeighbours(currentNode, targetNode, gridNodes, openNodeList, closedNodeHashSet, instantiatedRoom);
        }
        return null;
    }


    /// <summary>
    ///  Создание стек<Vector3>, содержащий путь перемещения
    /// </summary>
    private static Stack<Vector3> CreatePathStack(Node targetNode, Room room)
    {
        Stack<Vector3> movementPathStack = new Stack<Vector3>();

        Node nextNode = targetNode;

        // получить среднюю точку ячейки
        Vector3 cellMidPoint = room.instantiatedRoom.grid.cellSize * 0.5f;
        cellMidPoint.z = 0f;

        while (nextNode != null)
        {
            // Преобразовать положение сетки в мировое положение
            Vector3 worldPosition = room.instantiatedRoom.grid.CellToWorld(new Vector3Int(nextNode.gridPosition.x + room.templateLowerBounds.x,
                nextNode.gridPosition.y + room.templateLowerBounds.y, 0));

            // установка мирового положения на середину ячейки сетки
            worldPosition += cellMidPoint;

            movementPathStack.Push(worldPosition);

            nextNode = nextNode.parentNode;
        }
        return movementPathStack;
    }

    /// <summary>
    /// Оценка соседних узлов
    /// </summary>
    private static void EvaluateCurrentNodeNeighbours(Node currentNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, 
        HashSet<Node> closedNodeHashSet, InstantiatedRoom instantiatedRoom)
    {
        Vector2Int currentNodeGridPosition = currentNode.gridPosition;

        Node validNeighbourNode;

        // цикл во всех направлениях
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                validNeighbourNode = GetValidNodeNeighbour(currentNodeGridPosition.x + i, currentNodeGridPosition.y + j, gridNodes, closedNodeHashSet, instantiatedRoom);

                if (validNeighbourNode != null)
                {
                    // Рассчёт новой стоимости для соседа
                    int newCostToNeighbour;

                    // получить штраф за передвижение
                    // Непроходимые пути имеют значение 0. Штраф за перемещение по умолчанию устанавливается в
                    // настройках и применяется к другим квадратам сетки.
                    int movementPenaltyForGridSpace = instantiatedRoom.aStarMovementPenalty[validNeighbourNode.gridPosition.x, 
                        validNeighbourNode.gridPosition.y];

                    newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, validNeighbourNode) + movementPenaltyForGridSpace;

                    bool isValidNeighbourNodeInOpenList = openNodeList.Contains(validNeighbourNode);

                    if (newCostToNeighbour < validNeighbourNode.gCost || !isValidNeighbourNodeInOpenList)
                    {
                        validNeighbourNode.gCost = newCostToNeighbour;
                        validNeighbourNode.hCost = GetDistance(validNeighbourNode, targetNode);
                        validNeighbourNode.parentNode = currentNode;

                        if (!isValidNeighbourNodeInOpenList)
                        {
                            openNodeList.Add(validNeighbourNode);
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// Возвращает расстояние между узлами NodeA и NodeB
    /// </summary>
    private static int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int dstY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);  // 10 используется вместо 1, а 14 - это приближение Пифагора SQRT(10*10 + 10*10)
        return 14 * dstX + 10 * (dstY - dstX);
    }

    /// <summary>
    /// вычислить соседний узел в Neighbouroutnodexposition, соседний узел yPosition, используя
    /// указанные узлы сетки, хэш-набор закрытых узлов и созданную комнату.Возвращает значение null, если узел недействителен
    /// </summary>
    private static Node GetValidNodeNeighbour(int neighbourNodeXPosition, int neighbourNodeYPosition, GridNodes gridNodes, HashSet<Node> closedNodeHashSet, InstantiatedRoom instantiatedRoom)
    {
        // Если позиция соседнего узла находится за пределами сетки, то возвращает значение null
        if (neighbourNodeXPosition >= instantiatedRoom.room.templateUpperBounds.x - instantiatedRoom.room.templateLowerBounds.x || 
            neighbourNodeXPosition < 0 || 
            neighbourNodeYPosition >= instantiatedRoom.room.templateUpperBounds.y - instantiatedRoom.room.templateLowerBounds.y || 
            neighbourNodeYPosition < 0)
        {
            return null;
        }

        // Получить соседний узел
        Node neighbourNode = gridNodes.GetGridNode(neighbourNodeXPosition, neighbourNodeYPosition);

        // нет ли препятствий в этом положении
        int movementPenaltyForGridSpace = instantiatedRoom.aStarMovementPenalty[neighbourNodeXPosition, neighbourNodeYPosition];

        // нет ли подвижного препятствия в этом положении
        int itemObstacleForGridSpace = instantiatedRoom.aStarItemObstacles[neighbourNodeXPosition, neighbourNodeYPosition];


        // если сосед является препятствием или сосед находится в закрытом списке, то null
        if (movementPenaltyForGridSpace == 0 || itemObstacleForGridSpace == 0 || closedNodeHashSet.Contains(neighbourNode))
        {
            return null;
        }
        else
        {
            return neighbourNode;
        }

    }
}